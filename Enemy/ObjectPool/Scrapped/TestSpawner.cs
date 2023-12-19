using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

public class TestSpawner : MonoBehaviour
{

    [SerializeField] private EnemyPoolObject _enemyPoolObject;
    [SerializeField] private float spawnInterval = 1.0f;
    [HideInInspector] public bool enemyWave = true;
    
    [HideInInspector] public int currentEnemies = 0;
    public int poolSize = 3;
    
    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0, spawnInterval);
        // StartCoroutine(SpawnEnemies());
    }

    // private void Update()
    // {
    //     StartCoroutine(RemoveEnemies());
    // }

    private IEnumerator SpawnEnemies()
    {
        if (currentEnemies >= poolSize) 
            yield return this;
        
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = _enemyPoolObject.GetObject();
            enemy.transform.position = transform.position;
            currentEnemies++;
        }
    
        // yield return new WaitForSeconds();
    }
    private void SpawnEnemy()
    {
        if (currentEnemies >= poolSize) //It stops at three enemies
            return;
    
        GameObject enemy = _enemyPoolObject.GetObject();
        enemy.transform.position = transform.position;
        currentEnemies++;
    }
    
    public void ReturnEnemy(GameObject enemy)
    {
        enemy.SetActive(false);
        enemy.transform.SetParent(transform);
    
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyHealth.currentHealth = enemyHealth.healthData.Health;
        
        _enemyPoolObject.ReturnObject(enemy);
        currentEnemies--;
    }
    
    private IEnumerator RemoveEnemies()
    {
        // enemyWave = true;
        Debug.Log("var");
        yield return new WaitForSeconds(3f);
        // gameObject.SetActive(false);
        // this.gameObject.activeInHierarchy(false);
    }
    
    

}

