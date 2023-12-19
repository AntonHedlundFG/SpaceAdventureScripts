using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine.Animations.Rigging;

public class SpiderAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Animator _animator;
    [SerializeField] private RigBuilder _rigBuilder;
    [SerializeField] private CapsuleCollider _collider;
    [SerializeField] private string _mothershipDmgSound;
    [SerializeField] private string _playerDmgSound;
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Color _alertColor;
    [SerializeField] private SpiderAnimation _spiderAnimation;

    private Transform _atkTarget;
    private List<Transform> _mothershipAtkLocations;
    private Transform _mothership;
    [SerializeField] private Transform _currentTarget;
    private List<GameObject> _players = new List<GameObject>();
    private float _movementSpeed => Random.Range(_enemyData.MinSpeed, _enemyData.MaxSpeed);
	private float _fovRad => _enemyData.DetectionConeDegrees * Mathf.Deg2Rad;

	private GameObject _nearbyPlayer;
    private bool _isChasingPlayer = false;
    private bool _isAttacking = false;

    private float _outOfBoundsChaseTime = 3f;
    private bool _isChangingChaseTarget = false;

    private bool _isStunned = false;

    private Coroutine _slowRoutine;
    private Coroutine _stunRoutine;
    private float _lastPathingTime;

    void Start()
    {
        if (_spiderAnimation == null)
        {
            _spiderAnimation = GetComponent<SpiderAnimation>();
        }

        if (_renderer == null)
            _renderer = GetComponent<SkinnedMeshRenderer>();
        //_renderer.materials[3].color = _alertColor; //REMOVED TEMPORARILY
        _agent.speed = _movementSpeed;

        if (GameObject.FindGameObjectWithTag("Mothership"))
            _mothership = GameObject.FindGameObjectWithTag("Mothership").transform;

        GameObject _mothershipGO = GameObject.FindGameObjectWithTag("Mothership");

        _mothershipAtkLocations = _mothershipGO?.GetComponent<MothershipAtkLocations>().GetAtkLocations();

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) 
        {
            _players.Add(player);
        }

        if (_agent == null) 
            _agent = GetComponent<NavMeshAgent>();

        if (_mothershipGO != null)
        {
            _currentTarget = GetRandomAtkLocation();
            Debug.Log(_currentTarget.name);
        }
        else
            _currentTarget = _players[0].transform;

        

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
        if (!_isAttacking) 
            MoveTowardsTarget();
        
        if (_isChasingPlayer) 
        {
            float dist = Vector3.Distance(_currentTarget.transform.position, transform.position);
            if (dist >= _enemyData.MaxChaseDist && !_isChangingChaseTarget) 
            {
                _isChangingChaseTarget = true;
                Invoke("ChaseMothership", _outOfBoundsChaseTime);
            }
        }

        if (_isChasingPlayer) return;

        _nearbyPlayer = GetPlayerInRange();

        if (_nearbyPlayer && !_isChasingPlayer) 
        {
            if (!IsPlayerInVision(_nearbyPlayer)) return;
            ChasePlayer(_nearbyPlayer);
        }
    }

    private void SetIsWalking()
    {
        if (!_isAttacking)
        {
            _agent.isStopped = false;
            _rigBuilder.enabled = true;
        }
        else
        {
            _agent.isStopped = true;
            _rigBuilder.enabled = false;
        }
    }

    private GameObject GetPlayerInRange()
    {
        foreach (GameObject player in _players) 
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            
            if (player.GetComponent<HealthComponent>().IsDead)
                continue;
            if (dist < _enemyData.DetectRange)
                return player;
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
        HealthComponent hc;
        if (_currentTarget != null && _currentTarget.TryGetComponent<HealthComponent>(out hc) && hc.IsDead)
        {
            _currentTarget = GetRandomAtkLocation();
            return;
        }
        

        float dist = Vector3.Distance(_currentTarget.position, transform.position);

        if (dist <= _enemyData.AttackRange) {
            _isAttacking = true;
            _atkTarget = _currentTarget;

            if (_animator != null)
            {
                _animator.SetTrigger("attackTrigger");
            }
            Invoke("AttackTarget", _enemyData.AttackCooldown);
        }
        else {
            _isAttacking = false;
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
        _agent.isStopped = false;
        _isAttacking = false;
        _currentTarget = player.transform;
        _isChasingPlayer = true;
        _agent.speed = _enemyData.AggroSpeed;
    }

    private void ChaseMothership()
    {
        _isChangingChaseTarget = false;
        _agent.isStopped = false; //"Resume" bug error
        _isAttacking = false;
        _currentTarget = _mothership.transform;
        _currentTarget = GetRandomAtkLocation();
        transform.LookAt(_currentTarget);
        UpdateRotation();
        _isChasingPlayer = false;
        _agent.speed = _movementSpeed;
    }

    private Transform GetRandomAtkLocation()
    {
        /*
        foreach (Transform transforms in _mothershipAtkLocations)
            transform.LookAt(transforms);
        */
        int r = Random.Range(0, _mothershipAtkLocations.Count);
        
        return _mothershipAtkLocations[r];
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
            _currentTarget = GetRandomAtkLocation();
        }

        
    }

    private bool IsPlayerInVision(GameObject player)
    {
        Vector3 dirToTarget = ( player.transform.position - transform.position ).normalized;
		float angleRad = AngleBetweenNormalizedVectors( transform.forward, dirToTarget );
		return angleRad < _fovRad / 2;
    }

    private float AngleBetweenNormalizedVectors( Vector3 a, Vector3 b ) 
    {
		return Mathf.Acos( Mathf.Clamp( Vector3.Dot( a, b ), -1, 1 ) );
	}

    public void ApplySlow(float multiplier, float duration)
    {
        if (_slowRoutine != null)
        {
            StopCoroutine(_slowRoutine);
        }
        StartCoroutine(SlowRoutine(multiplier, duration));
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
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        _spiderAnimation.enabled = false;
        _agent.speed = 0;
        _isStunned = true;
        _animator.SetFloat("speed", 0f);
        // and make make it spaz?
        yield return new WaitForSeconds(duration);
        _animator.SetFloat("speed", 1f);
        _agent.speed = _movementSpeed;
        _isStunned = false;
        _spiderAnimation.enabled = true;
    }
    
    private void OnEnable()
    {
        _agent.isStopped = false;
        _rigBuilder.enabled = true;
        _collider.enabled = true;
    }
}
