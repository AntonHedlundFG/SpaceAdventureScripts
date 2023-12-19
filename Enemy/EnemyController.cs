using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyController : MonoBehaviour
{
    // Component references
    [Header("Component references")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private EnemyData _enemyData;
    [SerializeField] private Animator _animator;

    // Variable settings
    private float _movementSpeed; //This is now defined in the Start method so slow effects can know what the "original" setting was. /Anton
	private float _fovRad => _enemyData.DetectionConeDegrees * Mathf.Deg2Rad;

    // External references
    private List<GameObject> _players = new List<GameObject>();
	private GameObject _nearbyPlayer;
    private Transform _mothership;
    public Transform CurrentTarget;

    public List<GameObject> GetPlayerRef() { return _players; }
    public Transform GetMothershipRef() { return _mothership; }

    private Coroutine _slowRoutine;

    // Death Event
    // public delegate void OnEnemyDestroy();
    // public static event OnEnemyDestroy enemyDestroyed;

    public UnityEvent OnEnemyAlerted;
    
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Mothership"))
            _mothership = GameObject.FindGameObjectWithTag("Mothership").transform;

        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player")) {
            _players.Add(player);
        }
        if (_mothership != null)
            CurrentTarget = _mothership;
        else
            CurrentTarget = _players[0].transform;

        if (_agent == null) _agent = GetComponent<NavMeshAgent>();

        _movementSpeed = Random.Range(_enemyData.MinSpeed, _enemyData.MaxSpeed);
        _agent.speed = _movementSpeed;
    }

    public void DealDamageToTarget()
    {
        if (CurrentTarget.GetComponent<HealthComponent>())
            CurrentTarget.GetComponent<HealthComponent>().TakeDamage(_enemyData.Damage);
    }

    public void ChangeTarget(GameObject target)
    {
        CurrentTarget = target.transform;
        OnEnemyAlerted.Invoke();
    }

    public Animator GetAnimatorRef() { return _animator; }
    public EnemyData GetEnemyDataRef() { return _enemyData; }
    public NavMeshAgent GetNavAgentRef() { return _agent; }

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
}