using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDied : Decision
{
    private EnemyData _enemyData;
    private EnemyController _enemyController;
    private PlayerHealth _healthComponent;
    private GameObject _target;

    public override bool CheckDecision() 
    {
        if (_healthComponent.Health <= 0)
            return true;
        else 
            return false;
    }

    public override void OnEnter(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.GetEnemyDataRef();
        _healthComponent = _enemyController.CurrentTarget.GetComponent<HealthComponent>().GetPlayerHealth();
    }
    public override void OnExit() 
    {
        Debug.Log("Triggering TargetDied");
    }
}
