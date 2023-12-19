using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private AllResources _allResources;
    [SerializeField] private List<GameObject> _spawnPositions;
    [SerializeField] private List<GameObject> _objectPool;
    [SerializeField] private int _initialSpawnAmount = 4;
    //[SerializeField] private UI_ResourceSystem _UIResourceSystem;

    void Start()
    {
        for (int i = 0; i < _spawnPositions.Count; i++) {
            if (_spawnPositions[i] == null) _spawnPositions.Remove(_spawnPositions[i]);
            else {
                GameObject gO = _spawnPositions[i];       
                gO.GetComponent<ResourceController>().InitResourceObject(this);
                _objectPool.Add(gO);
            }
        }

        _initialSpawnAmount = Mathf.Clamp(_initialSpawnAmount, 0, _spawnPositions.Count - 1);

        for (int i = 0; i < _initialSpawnAmount; i++) {
            SpawnRandomResource();
        }
    }

    private void SpawnRandomResource()
    {
        GameObject gO = _objectPool[Random.Range(0, _objectPool.Count)];
        gO.GetComponent<ResourceController>().SpawnObject();
        _objectPool.Remove(gO);
    }

    public void ReturnSpawnObject(GameObject gObject)
    {
        SpawnRandomResource();
        _objectPool.Add(gObject);
    }
    /*
    public void AddResourceToUI(int amount)
    {
        _UIResourceSystem.UpdateResourceSlider(amount);
    }
    */
}
