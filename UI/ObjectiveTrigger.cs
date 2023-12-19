using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveTrigger : MonoBehaviour
{
    [Header("Assign PlayerSpawner Object Here")]
    [SerializeField] private WaypointManager playerSpawnerObj;

    [Header("Assign NextWaypoint position and script Here. Leave empty if last objective")]
    [SerializeField] private Transform nextPosition;
    [SerializeField] private ObjectiveTrigger nextWp;

    public bool pointActive;

    public void InitiateNewWaypoint()
    {
        playerSpawnerObj.AssignNewObjectivePoint(this.transform);
        pointActive = true;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.transform.CompareTag("Player") && pointActive && nextWp != null)
        {
            this.GetComponent<BoxCollider>().enabled = false;
            playerSpawnerObj.AssignNewObjectivePoint(nextPosition);
            nextWp.pointActive = true;
            pointActive = false;
        }else if(other.transform.CompareTag("Player"))
        {
            this.GetComponent<BoxCollider>().enabled = false;
            playerSpawnerObj.AssignNewObjectivePoint(null);
            pointActive = false;
            gameObject.SetActive(false);
        }
    }
}
