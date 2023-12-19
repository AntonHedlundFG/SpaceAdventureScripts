using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Animations.Rigging;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpiderAIPatrol _spiderAIPatrol;
    [SerializeField] private SpiderAI _spiderAI;
    [SerializeField] private RigBuilder _rigBuilder;
    [SerializeField] private SpiderAnimation _procedAnim;
    bool isDying = false;

    public int currentHealth;
    private bool _agent;
    public EnemyData healthData;
    private EnemyPool _pool;

    public UnityEvent<int> OnEnemyHealthChange;
    public UnityEvent OnEnemyDeath;

    private Coroutine _dotRoutine;
    
    public int Health
    {
        get 
        {
            return currentHealth;
        }
        set 
        {
            currentHealth = value;
            OnEnemyHealthChange.Invoke(currentHealth);
            if (Health <= 0)
                OnEnemyDeath.Invoke();
        }
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _pool = GetComponent<EnemyPool>();
        
        Health = healthData.Health;
    }

    private void OnEnable()
    {
        if (_spiderAIPatrol != null)
            _spiderAIPatrol.enabled = true;
        else if (_spiderAI != null)
            _spiderAI.enabled = true;
    
        Health = healthData.Health;
        OnEnemyDeath.AddListener(EnemyDeath);
    }

    private void OnDisable()
    {
        OnEnemyDeath.RemoveListener(EnemyDeath);
    }


    public void DealDamage(int damage, GameObject instigator)
    {        
        if (instigator != null)
        {
            if (_spiderAIPatrol != null)
                _spiderAIPatrol.ChasePlayer(instigator);
            else if (_spiderAI != null)
                _spiderAI.ChasePlayer(instigator);
        }
        
        Health -= damage;
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/Electric Hit Enemy");
    }

    public void DealDoTDamage(int damage, int duration)
    {
        if (_dotRoutine != null)
        {
            StopCoroutine(_dotRoutine);
        }

        if (gameObject.activeSelf)
        {
            _dotRoutine = StartCoroutine(DoTRoutine(damage, duration));
        }
    }

    private IEnumerator DoTRoutine(int damage, int duration)
    {
        for (int i = duration; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            Health -= damage;
        }
    }
    
    private void EnemyDeath()
    {
        if (!isDying)
        {
            isDying = true;
            if (_spiderAIPatrol != null)
            {
                _spiderAIPatrol.InitDeath();
                Invoke("DelayDeath", 2f);
            }
            else if (_spiderAI != null)
            {
                _spiderAI.InitDeath();
                Invoke("DelayDeath", 2f);
            }
            // TODO: Add a proper death loop
        }
    }

    private void DelayDeath()
    {
        Health = healthData.Health;
        OnEnemyDeath.RemoveAllListeners();
        OnEnemyHealthChange.RemoveAllListeners();
        isDying = false;

        if (_pool != null)
            _pool.ReturnEnemyToPool(gameObject);
        else 
            Destroy(gameObject);
    }

    public void SetPool(EnemyPool pool) //Implementing for later
    {
        _pool = pool;
    }
}
