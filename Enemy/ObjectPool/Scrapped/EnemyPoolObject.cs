using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Unity.VisualScripting;

//TODO Fix a bug when EnemyPrefab randomly spawns at (0, 0, 0)
//TODO look up more info on QUEUE 
//TODO Implement SetActive(false) instead of destroy
//TODO if the children are inactive then parent should be inactive as well. 

public class EnemyPoolObject : MonoBehaviour
{
    /// <summary>
    /// This script is scrapped
    /// </summary>

    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int poolSize = 3;
    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    [SerializeField] private EnemyPoolObject _enemyPoolObject;
    [SerializeField] private float spawnInterval = 1.0f;
    [SerializeField] private float DelaySpawn;
    [SerializeField] private float spawnMaxRadius;
    
    [HideInInspector] public int currentEnemies = 0;
    
    private void Start()
    {
        availableObjects = new Queue<GameObject>();
        
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab, this.transform.position, quaternion.identity);
            obj.transform.SetParent(transform);
            obj.SetActive(false); //If I can make it true. Then I might hinder it some other way
            availableObjects.Enqueue(obj);
        }
        
        InvokeRepeating("SpawnEnemy", 0, spawnInterval);
    }

    // private void Update()
    // {
    //     Invoke("SetParentInactiveIfAllChildrenAreInactive", 4.0f);
    //     // KillWaves();
    // }
    //
    // private void KillWaves()
    // {
    //     bool ifAllChildrenAreInactive = AreAllChildrenActive(this.transform);
    //     if (ifAllChildrenAreInactive == false)
    //     {
    //         this.gameObject.SetActive(false);
    //     }
    //     // Destroy(this.gameObject);
    // }
    // public void SetParentInactiveIfAllChildrenAreInactive(Transform parent)
    // {
    //     bool allChildrenInactive = true;
    //     foreach (Transform child in parent.GetComponentsInChildren<Transform>(includeInactive: false))
    //     {
    //         if (child.gameObject.activeSelf)
    //         {
    //             allChildrenInactive = false;
    //             break;
    //         }
    //     }
    //     if (allChildrenInactive)
    //     {
    //         this.gameObject.SetActive(false);
    //     }
    // }
    // public bool AreAllChildrenActive(Transform parent)
    // {
    //     for (int i = 0; i < parent.childCount; i++)
    //     {
    //         Transform child = parent.GetChild(i);
    //         if (!child.gameObject.activeSelf)
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }
    //
    // public bool AreAllChildrenActive(Transform parent) //Old method
    // {
    //     foreach (Transform child in parent.GetComponentsInChildren<Transform>(/*includeInactive: false*/))
    //     {
    //         if (child.gameObject.activeSelf == false)
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }

    private void SpawnEnemy()
    {
        if (currentEnemies >= poolSize) //It stops at three enemies
            return;
        
        GameObject enemy = /*_enemyPoolObject.*/GetObject();
        enemy.transform.position = Random.insideUnitSphere * spawnMaxRadius + transform.position;
        // enemy.transform.position = transform.position;
        currentEnemies++;
    }
    
    public GameObject GetObject()
    {
        if (availableObjects.Count == 0)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            availableObjects.Enqueue(obj);
            return obj;
        }
        
        GameObject objToUse = availableObjects.Dequeue();
        objToUse.SetActive(true);
        // StartCoroutine(ActivateObjectAfterDelay(objToUse, DelaySpawn));
        
        return objToUse;
    }

    private IEnumerator ActivateObjectAfterDelay(GameObject obj, float delaySpawn)
    {
        yield return new WaitForSeconds(delaySpawn);
        obj.SetActive(true);
    }

    public void ReturnEnemy(GameObject enemy)
    {
        // enemy.SetActive(false);
        // enemy.transform.SetParent(transform);
    
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyHealth.currentHealth = enemyHealth.healthData.Health;
        
        _enemyPoolObject.ReturnObject(enemy);
        currentEnemies--;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        availableObjects.Enqueue(obj);
    }
    
}
