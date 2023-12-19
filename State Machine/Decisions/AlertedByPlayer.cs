using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertedByPlayer : Decision
{
    private EnemyData _enemyData;
    private EnemyController _enemyController;
    private bool isAlerted = false;

    public override bool CheckDecision() 
    {
        return isAlerted;
    }

    public override void OnEnter(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.GetEnemyDataRef();
        _enemyController.OnEnemyAlerted.AddListener(TriggerIsAlerted);
    }

    private void TriggerIsAlerted()
    {
        isAlerted = true;
        Debug.Log("Alerted");
    }

    public override void OnExit() 
    {
        _enemyController.OnEnemyAlerted.RemoveListener(TriggerIsAlerted);
    }
}
