using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Animations.Rigging;

public class SpiderAIPatrol : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Animator _animator;
    [SerializeField] private RigBuilder _rigBuilder;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private string _mothershipDmgSound;
    [SerializeField] private string _playerDmgSound;
    // [SerializeField] private SkinnedMeshRenderer _renderer; // used for the alerted eye color change // deprecated atm
    // [SerializeField] private Color _alertColor;
    [SerializeField] private SpiderAnimation _spiderAnimation;

    private Transform _atkTarget;
    private Color _defaultColor;
    private Vector3 _originalPos;    
    private bool _isStunned = false;
    private Vector3 _patrolPos;
    private List<Transform> _mothershipAtkLocations;
    private Transform _mothership;
    private Transform _currentTarget;
    private List<GameObject> _players = new List<GameObject>();
    private float _movementSpeed => Random.Range(_enemyData.MinSpeed, _enemyData.MaxSpeed);
	private float _fovRad => _enemyData.DetectionConeDegrees * Mathf.Deg2Rad;

	private GameObject _nearbyPlayer;
    private bool _isChasingPlayer = false;
    private bool _isAttacking = false;

    private float _outOfBoundsChaseTime => _enemyData.OutOfBoundsChaseTime;
    private bool _returningToPatrol = false;
    private float _lastPathingTime;

    private Coroutine _slowRoutine;
    private Coroutine _stunRoutine;
    private Coroutine _patrolRoutine;

    void Awake()
    {
        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();
        if (_spiderAnimation == null)
            _spiderAnimation = GetComponent<SpiderAnimation>();
    }

    void Start()
    {
        _patrolPos = transform.position;
        _agent.speed = _movementSpeed;
        _patrolRoutine = StartCoroutine(PatrolRandomSpot());
        _lastPathingTime = Time.time;
    }

    private void GetPlayerRefs()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) 
        {
            _players.Add(player);
        }
    }

    public void SetPlayerRefs(GameObject player)
    {
        _players.Add(player);
    }

    void Update()
    {
        if (_isStunned)
        {
            _agent.isStopped = true;
            return;
        }
        UpdateRotation();
        AttackTarget();
        SetIsWalking();

        if (_returningToPatrol)
            CheckDistanceToPatrolSpot();
        
        if (!_isAttacking) 
            MoveTowardsTarget();
        
        if (_isChasingPlayer) 
            CheckTargetDistance();
        else
        {
            _nearbyPlayer = GetPlayerInRange();
        }
    
        if (_nearbyPlayer && !_isChasingPlayer && !_isAttacking)
        {
            ChasePlayer(_nearbyPlayer);
        }
    }

    private void CheckDistanceToPatrolSpot()
    {
        float dist = Vector3.Distance(_patrolPos, transform.position);

        if (dist <= _enemyData.AttackRange)
        {
            _returningToPatrol = false;
            _patrolRoutine = StartCoroutine(PatrolRandomSpot());
        }
    }

    private IEnumerator PatrolRandomSpot()
    {
        Vector3 random = transform.position + Random.insideUnitSphere * Random.Range(1f, 4f);
        Vector3 location = new Vector3(random.x, transform.position.y, random.z);
        _agent.SetDestination(location);
        yield return new WaitForSeconds(Random.Range(2f, 6f));
        _patrolRoutine = StartCoroutine(PatrolRandomSpot());
    }

    private void ReturnToPatrolSpot()
    {
        _returningToPatrol = true;
        _isChasingPlayer = false;
        _agent.SetDestination(_patrolPos);
    }

    private void CheckTargetDistance()
    {
        float dist = Vector3.Distance(_currentTarget.transform.position, transform.position);

        if (dist >= _enemyData.MaxChaseDist && !_returningToPatrol) 
        {
            ReturnToPatrolSpot();
        }
    }

    private void SetIsWalking()
    {
        if (!_isAttacking)
        {
            _rigBuilder.enabled = true;
            _agent.isStopped = false;
        }
        else
        {
            _rigBuilder.enabled = false;
            _agent.isStopped = true;
        }
    }

    private GameObject GetPlayerInRange()
    {
        foreach (GameObject player in _players)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            
            if (player.GetComponent<HealthComponent>().IsDead)
                continue;
            if (dist <= _enemyData.DetectRange)
            {
                return player;
            }
        }
        return null;
    }

    void UpdateRotation()
    {
        var turnTowardNavSteeringTarget = _agent.steeringTarget;
        
        Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
        Vector3 lookDirection = new Vector3(direction.x, 0, direction.z);
        if (lookDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
        }
    }

    private void AttackTarget()
    {
        if (_currentTarget != null && !_isStunned)
        {
            if (_currentTarget.GetComponent<HealthComponent>().IsDead)
            {
                ReturnToPatrolSpot();
                _currentTarget = null;
                return;
            }

            float dist = Vector3.Distance(_currentTarget.position, transform.position);

            if (dist <= _enemyData.AttackRange && !_isAttacking)
            {
                _atkTarget = _currentTarget;
                _isAttacking = true;

                if (_animator != null)
                {
                    _animator.SetTrigger("attackTrigger");
                }
                Invoke("AttackTarget", _enemyData.AttackCooldown);
            }
        }
    }

    public void SetIsAttackingFalse()
    {
        _isAttacking = false;
    }

    public void InitDeath()
    {
        _collider.enabled = false;
        _agent.SetDestination(transform.position);
        _agent.isStopped = true;
        _rigBuilder.enabled = false;
        _animator.SetTrigger("deathTrigger");
        this.enabled = false;
    }

    private void MoveTowardsTarget()
    {
        if (_currentTarget == null || _isAttacking) return;
        _agent.isStopped = false;
        if (Time.time < _lastPathingTime + 0.1f)
        {
            return;
        }

        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, _currentTarget.position, NavMesh.AllAreas, path))
        {
            _agent.SetPath(path);
            _lastPathingTime = Time.time;
        }
    }

    public void ChasePlayer(GameObject player)
    {
        if (_patrolRoutine != null)
            StopCoroutine(_patrolRoutine);
        _agent.isStopped = false;
        _isAttacking = false;
        _currentTarget = player.transform;
        _isChasingPlayer = true;
    }
    
    public void DealDamageToTarget()
    {
        HealthComponent hc = _atkTarget.GetComponentInParent<HealthComponent>();
        if (hc == null)
        {
            return;
        }
        if (hc.gameObject.CompareTag("Mothership"))
        {
            FMODUnity.RuntimeManager.PlayOneShot(_mothershipDmgSound);
        }
        else
        {
            FMODUnity.RuntimeManager.PlayOneShot(_playerDmgSound);
        }

        hc.TakeDamage(_enemyData.Damage);

        if (hc.IsDead)
        {
            ReturnToPatrolSpot();
        } 
    }

    public void ApplySlow(float multiplier, float duration)
    {
        if (_slowRoutine != null)
        {
            StopCoroutine(_slowRoutine);
        }
        _slowRoutine = StartCoroutine(SlowRoutine(multiplier, duration));
    }

    private IEnumerator SlowRoutine(float multiplier, float duration)
    {
        _agent.speed = _movementSpeed * multiplier;   
        yield return new WaitForSeconds(duration);
        _agent.speed = _movementSpeed;
    }

    public void ApplyStun(float duration)
    {
        if (_stunRoutine != null)
        {
            StopCoroutine(_stunRoutine);
        }
        _stunRoutine = StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        _spiderAnimation.enabled = false;
        _agent.speed = 0;
        _isStunned = true;
        _animator.SetFloat("speed", 0f);
        yield return new WaitForSeconds(duration);
        _animator.SetFloat("speed", 1f);
        _agent.speed = _movementSpeed;
        _isStunned = false;
        _spiderAnimation.enabled = true;
    }

    public void OnEnable()
    {
        _agent.isStopped = false;
        _rigBuilder.enabled = true;
        _collider.enabled = true;
    }
}
