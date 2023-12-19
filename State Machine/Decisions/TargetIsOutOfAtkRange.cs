using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIsOutOfAtkRange : Decision
{
    private EnemyData _enemyData;
    private EnemyController _enemyController;
    private float _fovRad => _enemyData.DetectionConeDegrees * Mathf.Deg2Rad;

    public override bool CheckDecision() 
    {
        float dist = Vector3.Distance(_enemyController.CurrentTarget.position, transform.position);

        if (dist >= _enemyData.AttackRange) {
            return true;
        }
        return false;
    }

    public override void OnEnter(EnemyController enemyController)
    {
        if (enemyController == null) return;
        _enemyController = enemyController;
        _enemyData = _enemyController.GetEnemyDataRef();
    }
    public override void OnExit() { }
}
