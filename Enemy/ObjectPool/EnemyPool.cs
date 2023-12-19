using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textComponent;
    public GameObject enemyPrefab;
    [SerializeField] private int maxEnemiesToPool;
    
    [Header("Randomly spawns from all transforms")]
    [SerializeField] private int spawnDuringNight;
    [SerializeField] private int spawnDuringNightAgain; 
    [SerializeField] private int spawnContinuously;
    private int _diffBetweenQnSpawn;
    private int _diffMaxPoolSize;
    private float delaySpawn; 
    
    
    [Header("How many to spawn after returning engine/Last Stand event")]
    [SerializeField] private int spawnLastStand;
    [SerializeField] private float setLastStandTimer;
    [HideInInspector] public float countsDown;
    [HideInInspector] public bool startLastStand = false;
    
    
    [Header("Randomly spawns enemies from this list of transforms")]
    [SerializeField] private List<Transform> enemySpawnPoints;

    [SerializeField] private DayNightCycle _dayNightCycle;
    [SerializeField] private DayNightData _dayNightData;
    private Queue<GameObject> enemyQueue;

    [Header("Game events")]
    [SerializeField] private EventSO _afterReturnedFirstPickUp;
    [SerializeField] private EventSO _afterReturnedSecondPickUp;
    [SerializeField] private EventSO _lastStandWinCondition;
    [SerializeField] private EventSO _dayBegins;    
    [SerializeField] private EventSO _nightBegins;    
    
    private void Awake()
    {
        if (_dayNightCycle != null)
        {
            _dayNightCycle.spawnDuringNight = spawnDuringNight;
            _dayNightCycle.spawnDuringNightAgain = spawnDuringNightAgain;
        }

        _dayNightData.ifContinuousSpawning = false;
        startLastStand = false;
        countsDown = setLastStandTimer;
        
        enemyQueue = new Queue<GameObject>(maxEnemiesToPool);
        SpawnEnemyPoolSizeDuringAwake();
    }
    private void Update()
    {
        if (_dayNightData.ifContinuousSpawning && spawnContinuously > 0) //Can only spawn during the night
            SpawnContinuously(spawnContinuously);
        
        if (startLastStand)
        {
            SpawnContinuously(spawnLastStand); 
            countsDown -= Time.deltaTime;
            if (0f >= countsDown)
            {
                countsDown = 0f;
                Debug.Log("You win!");
                _lastStandWinCondition?.Event.Invoke();
            }
        }
    } 
    private void OnEnable()
    {
        _afterReturnedSecondPickUp?.Event.AddListener(SpawnAfterReturningSecondPickUp);
    }
    private void OnDisable()
    {
        _afterReturnedSecondPickUp?.Event.RemoveListener(SpawnAfterReturningSecondPickUp);
    }
    private void SpawnAfterReturningSecondPickUp()
    {
        startLastStand = true;
    }
    private void SpawnEnemyPoolSizeDuringAwake()
    {
        for (int i = 0; i < maxEnemiesToPool; i++)
        {
            GameObject enemyObject = Instantiate(enemyPrefab);
            enemyObject.SetActive(false);
            enemyQueue.Enqueue(enemyObject);
        }
    }
    public void SpawnEnemyAtRandomTransforms(int howManyToSpawn) 
    {
        if (howManyToSpawn > enemyQueue.Count) //If there are not enough to spawn
        {
            int diff = howManyToSpawn - enemyQueue.Count;
            howManyToSpawn -= diff;
            InsideSpawnMethod(howManyToSpawn);
        }
        else if (enemyQueue.Count > 0)
            InsideSpawnMethod(howManyToSpawn);
    }
    public void SpawnContinuously(int howManyToSpawn) // 5
    { //Pool size is 50 - 13 = 37
        _diffMaxPoolSize = maxEnemiesToPool; //15
        _diffBetweenQnSpawn = howManyToSpawn; //5
        _diffBetweenQnSpawn = enemyQueue.Count - _diffBetweenQnSpawn; // 15-5=10
        
        if (_diffBetweenQnSpawn <= enemyQueue.Count) //diff is 10 // enemyQueue is 15 as an example
        {
            int diffMaxPool = _diffMaxPoolSize; //is 15
            int diffQ = enemyQueue.Count; //is 15 or less

            int diffBetweenMaxPoolNEnemyQ = diffQ - diffMaxPool; //15-15 = 0
            diffBetweenMaxPoolNEnemyQ += howManyToSpawn; //0+5=5
            
            if (diffBetweenMaxPoolNEnemyQ <= howManyToSpawn) // if 5 or less
                InsideSpawnMethod(diffBetweenMaxPoolNEnemyQ);
        }
        else
            Debug.Log("Did not spawn Continuously. enemyQueue: " + enemyQueue.Count);
    }
    private void InsideSpawnMethod(int howManyToSpawn)
    {
        for (int i = 0; i < howManyToSpawn; i++)
        {
            GameObject enemyObject = enemyQueue.Dequeue();
            EnemyHealth eh = enemyObject.GetComponent<EnemyHealth>();
            
            eh.SetPool(this);

            int randomIndex = Random.Range(0, enemySpawnPoints.Count);
            enemyObject.transform.position = enemySpawnPoints[randomIndex].position;
            StartCoroutine(DelaySpawn(delaySpawn, enemyObject));
        }
    }
    private IEnumerator DelaySpawn(float delay, GameObject enemyObject)
    {
        yield return new WaitForSeconds(delay);
        enemyObject.SetActive(true);
    }
    public void ReturnEnemyToPool(GameObject enemyObject)
    {
        enemyObject.SetActive(false);
        enemyQueue.Enqueue(enemyObject);
    }
}
