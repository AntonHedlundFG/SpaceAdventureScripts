using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class QuestEventsManager : MonoBehaviour
{
    [Header("First pick up event")]
    [SerializeField] private EventSO _firstWingPickup;
    [SerializeField] private EventSO _afterReturnedFirstPickUp;
    
    [Header("Second pick up event")]
    [SerializeField] private EventSO _secondPickUp;
    [SerializeField] private EventSO _afterReturnedSecondPickUp;

    [HideInInspector] public bool ifFirstPickUp = false;
    [HideInInspector] public bool ifSecondPickUp = false;
    
    [SerializeField] private ObjectiveSystem taskSys;
    [SerializeField] private Sprite missionIcon;

    [SerializeField] private WaypointManager wpManager;

    [SerializeField] private Transform mothershipPos;
    [SerializeField] private Transform dungeon2;
    [SerializeField] private Transform part2Pos;
    
    private void Awake()
    {
        ifFirstPickUp = false;
        ifSecondPickUp = false;
    }

    private void OnEnable()
    {
        
        _firstWingPickup?.Event.AddListener(AfterFirstPickUp);
        _secondPickUp?.Event.AddListener(AfterSecondPickUp);
    }

    private void OnDisable()
    {
        _firstWingPickup?.Event.RemoveListener(AfterFirstPickUp);
        _secondPickUp?.Event.RemoveListener(AfterSecondPickUp);
    }

    private void AfterFirstPickUp()
    {
        taskSys.StartMission("Return To Mothership", "Go back to the mothership and return the part.", missionIcon);
        wpManager.AssignNewObjectivePoint(mothershipPos);
        ifFirstPickUp = true;

    }
    private void AfterSecondPickUp()
    {
        taskSys.StartMission("Return To Mothership", "Go back to the mothership and return the part.", missionIcon);
        wpManager.AssignNewObjectivePoint(mothershipPos);
        ifSecondPickUp = true;
    }

    public void AfterReturningFirstPickUp()
    {
        taskSys.StartMission("Investigate Mysterious Enterance", "Explore area to find the last ship part.", missionIcon);
        wpManager.AssignNewObjectivePoint(dungeon2);
        _afterReturnedFirstPickUp?.Event.Invoke();
        ifFirstPickUp = false;
        _afterReturnedFirstPickUp?.Event.RemoveAllListeners();
    }

    public void AfterReturningSecondPickUp()
    {
        taskSys.StartMission("Defend Ship", "Defend your ship from a final wave of enemies.", missionIcon);
        _afterReturnedSecondPickUp?.Event.Invoke();
        ifSecondPickUp = false;
        _afterReturnedSecondPickUp?.Event.RemoveAllListeners();
    }
    
}
