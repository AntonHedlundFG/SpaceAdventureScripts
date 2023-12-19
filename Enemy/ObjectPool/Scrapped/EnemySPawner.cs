using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
// using Random = System.Random;
using Random = UnityEngine.Random;

public partial class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int maxEnemies;
    [SerializeField] private int enemyIntervals;
    private float timer = 0f;
    private float t = 0f;
    private float spawnRate = 1f;
    
    private int spawnedEnemies = 0;

    private void Start() 
    {
         // EnemyAI.enemyDestroyed += DecrementSpawnedEnemies;
    }

    private void Update()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        t += Time.deltaTime;
        timer += Time.deltaTime;
        yield return new WaitForSeconds(enemyIntervals);
        
        if(timer >= spawnRate && spawnedEnemies < maxEnemies)
        {
            timer = 0f;
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)]; //UnityEngine.Random; Not System.Random;
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);


            StartCoroutine(SpawnEnemy());
            spawnedEnemies++;
        }
    }

    private IEnumerator DelaySeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    
    private void DecrementSpawnedEnemies()
    {
        spawnedEnemies--;
    }
}
