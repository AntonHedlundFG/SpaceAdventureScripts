using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Animator _animator;
    [SerializeField] private Material _slowMaterial;
    // [SerializeField] private bool isIdleState = true;
    
    private Material _defaultMaterial;
    private MeshRenderer _meshRenderer;

    private List<Transform> _mothershipAtkLocations;
    // private Transform _mothership;
    private Transform _currentTarget;
    private List<GameObject> _players = new List<GameObject>();
    private float _movementSpeed => Random.Range(_enemyData.MinSpeed, _enemyData.MaxSpeed);
	private float _fovRad => _enemyData.DetectionConeDegrees * Mathf.Deg2Rad;

	private GameObject _nearbyPlayer;
    private bool _isChasingPlayer = false;
    private bool _isAttacking = false;

    private float _outOfBoundsChaseTime = 3f;
    private bool _isChangingChaseTarget = false;



    private Coroutine _slowRoutine;
    //TODO idle/patrolling behaviour method 
    void Awake()
    {
        _meshRenderer = GetComponentInChildren<MeshRenderer>();
        _defaultMaterial = _meshRenderer.material;
        // if (GameObject.FindGameObjectWithTag("Mothership"))
        //     _mothership = GameObject.FindGameObjectWithTag("Mothership").transform;
    }

    private void Start()
    {
        GameObject _mothershipGO = GameObject.FindGameObjectWithTag("Mothership");
        // _mothership = _mothershipGO.transform;
        _mothershipAtkLocations = _mothershipGO.GetComponent<MothershipAtkLocations>().GetAtkLocations();

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            _players.Add(player);
        }

        if (_mothershipGO != null)
        {
            // _currentTarget = _mothership;
            _currentTarget = GetRandomAtkLocation();
        }
        else
            _currentTarget = _players[0].transform;

        if (_agent == null) 
            _agent = GetComponent<NavMeshAgent>();

        _agent.speed = _movementSpeed;
        
    }

    void Update()
    {
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
            if (_animator != null)
            {
                _animator.SetBool("isWalking", true);
            }
        } 
        else
        {
            _agent.isStopped = true;
            if (_animator != null)
            {
                _animator.SetBool("isWalking", false);
            }
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
        float dist = Vector3.Distance(_currentTarget.position, transform.position);

        if (dist <= _enemyData.AttackRange) {
            _isAttacking = true;

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
    private void MoveTowardsTarget()
    {
        if (_currentTarget == null) return;
        _agent.isStopped = false;
        _agent.SetDestination(_currentTarget.position);
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
        // _currentTarget = _mothership.transform;
        _currentTarget = GetRandomAtkLocation();
        transform.LookAt(_currentTarget);
        UpdateRotation();
        _isChasingPlayer = false;
        _agent.speed = _movementSpeed;
    }
    private Transform GetRandomAtkLocation()
    {
        foreach (Transform transforms in _mothershipAtkLocations)
            transform.LookAt(transforms);

        int r = Random.Range(0, _mothershipAtkLocations.Count);
        
        return _mothershipAtkLocations[r];
    }
    public void DealDamageToTarget()
    {
        HealthComponent hc = _currentTarget.GetComponentInParent<HealthComponent>();
        if (hc == null)
        {
            return;
        }
        //SOMETHING CHECK DEAD
        if (hc.IsDead)
        {
            // _currentTarget = _mothership;
            _currentTarget = GetRandomAtkLocation();
        }

        hc.TakeDamage(_enemyData.Damage);
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
        _meshRenderer.material = _slowMaterial;
        _agent.speed = _movementSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        _agent.speed = _movementSpeed;
        _meshRenderer.material = _defaultMaterial;
    }
    public void ApplyStun(float duration)
    {
        Debug.Log("Stunned for " + duration + " seconds.");
        // Destroy(this);

        //IMPLEMENT THIS
    }

    private IEnumerator DelayTime(float time)
    {
        yield return new WaitForSeconds(time);
    }



}
