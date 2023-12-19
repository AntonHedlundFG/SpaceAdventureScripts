using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(fileName="All Resources", menuName = "Resources/All Resources")]
public class AllResources : ScriptableObject
{
    [SerializeField] private List<Resource> _allResources;

    public List<Resource> GetAllResources() => new List<Resource>(_allResources);
}
