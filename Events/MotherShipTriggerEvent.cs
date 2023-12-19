using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipTriggerEvent : MonoBehaviour
{
    [SerializeField] private QuestEventsManager _questEventsManager;
    private void OnTriggerEnter(Collider other)
    {
        if (_questEventsManager.ifFirstPickUp)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                _questEventsManager.AfterReturningFirstPickUp();
        }
        
        if (_questEventsManager.ifSecondPickUp)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
                _questEventsManager.AfterReturningSecondPickUp();
        }
    }
}
