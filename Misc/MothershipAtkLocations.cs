using UnityEngine;
using System.Collections.Generic;

public class MothershipAtkLocations : MonoBehaviour
{
    [SerializeField] private List<Transform> atkLocations = new List<Transform>();

    public Transform GetRandomAtkLocation() 
    {
        int r = Random.Range(0, atkLocations.Count);
        return atkLocations[r];
    }

    public List<Transform> GetAtkLocations()
    {
        return atkLocations;
    }
}
