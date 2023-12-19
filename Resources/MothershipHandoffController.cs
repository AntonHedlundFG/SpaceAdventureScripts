using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipHandoffController : ResourceController
{
    [SerializeField] private QuestEventsManager _questManager;
    [SerializeField] private DayNightData _dayNightData;

    [SerializeField] private bool _onlyHandoffAtNight;

    public override void CollectResource()
    {
        if (_onlyHandoffAtNight && _dayNightData.isNight)
        {
            Debug.Log("Can't hand in at night");
            return;
        }

        if (_questManager.ifFirstPickUp) _questManager.AfterReturningFirstPickUp();
        if (_questManager.ifSecondPickUp) _questManager.AfterReturningSecondPickUp();
    }
}
