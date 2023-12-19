using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 3;
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private Transform _spawnPoint;
    private EnemyData startingHealth;

    private Queue<GameObject> _enemyPool;

    private void Start()
    {
        enemyPrefab = new GameObject();
        
        _enemyPool = new Queue<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.SetActive(false);
            _enemyPool.Enqueue(enemy);
        }
        InvokeRepeating("InstantiateEnemy", 0, spawnInterval);
        
        
    }

    private void InstantiateEnemy()
    {

        enemyPrefab = _enemyPool.Dequeue();
        enemyPrefab.SetActive(true);
        enemyPrefab.transform.position = _spawnPoint.position;
        // enemyPrefab.GetComponent<EnemyHealth>().currentHealth = startingHealth.Health;
        enemyPrefab.SetActive(true);
    }
    
    
    
}
