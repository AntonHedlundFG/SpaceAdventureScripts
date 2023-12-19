using UnityEngine;

public class IsInAttackRange : Decision
{
    private EnemyData _enemyData;
    private EnemyController _enemyController;

    public override bool CheckDecision() 
    {
        if (_enemyController == null) return false;
        float dist = Vector3.Distance(_enemyController.CurrentTarget.position, transform.position);

        if (dist <= _enemyData.AttackRange) {
            return true;
        }
        return false;
    }

    public override void OnEnter(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyData = _enemyController.GetEnemyDataRef();
    }

    public override void OnExit() 
    {
        // Debug.Log("Triggering IsInAttackRange");
    }
}
