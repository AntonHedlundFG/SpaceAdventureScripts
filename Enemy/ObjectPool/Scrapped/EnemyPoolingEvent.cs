using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class EnemyPoolingEvent : MonoBehaviour
{

    [SerializeField] private float waveDelay;
    [SerializeField] private int howManyToSpawn;
    public EnemyPool EnemyPool;
    // private EnemyPool _enemyPool;

    private void OnEnable()
    {
        // EnemyPool.OnEnemySpawned.AddListener(HandleEnemySpawned);
    }
    
    private void OnDisable()
    {
        // EnemyPool.OnEnemySpawned.RemoveListener(HandleEnemySpawned);
    }
    
    public void HandleEnemySpawned()
    {
        Debug.Log("Inside HandleEnemySpawned");
        StartCoroutine(SpawnEnemiesAgain());
    }

    private IEnumerator SpawnEnemiesAgain()
    {
        Debug.Log("EnemySpawnAgain!");
        yield return new WaitForSeconds(waveDelay);
        EnemyPool.SpawnEnemyAtRandomTransforms(howManyToSpawn);
    }

    private void SetPool(EnemyPool enemyPool)
    {
        EnemyPool = enemyPool;
    }
}
