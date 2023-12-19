using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIsOutOfRange : Decision
{
    private EnemyData _enemyData;
    private EnemyController _enemyController;
    [SerializeField] private float _outOfBoundsChaseTime = 3f;
    private float _fovRad;
    private Transform _target;
    private bool isReturning = false;
    private bool _isOutOfBounds = false;

    public override bool CheckDecision() 
    {
        CheckIsPlayerOutOfBounds();
        return _isOutOfBounds;
    }

    private void CheckIsPlayerOutOfBounds()
    {
        float dist = Vector3.Distance(_enemyController.CurrentTarget.position, transform.position);

        if (dist >= _enemyData.MaxChaseDist && !isReturning) {
            Invoke("ChangeState", _outOfBoundsChaseTime);
            isReturning = true;
        }
    }

    private void ChangeState()
    {
        _isOutOfBounds = true;
    }

    public override void OnEnter(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.GetEnemyDataRef();
        _fovRad = _enemyData.DetectionConeDegrees * Mathf.Deg2Rad;
    }

    public override void OnExit() 
    {
        Debug.Log("Triggering TargetIsOutOfRange");
    }
}
