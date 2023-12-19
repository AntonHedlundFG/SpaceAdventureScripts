using UnityEngine;
using UnityEngine.AI;

public class Attack : Action
{
    private Animator _animator;
    private EnemyData _enemyData;
    private EnemyController _enemyController;
    private NavMeshAgent _agent;

    private void AttackTarget()
    {
        float dist = Vector3.Distance(_enemyController.CurrentTarget.position, transform.position);

        if (dist <= _enemyData.AttackRange) 
        {
            _agent.isStopped = true;
            _animator.SetTrigger("attackTrigger");
            Invoke("AttackTarget", _enemyData.AttackCooldown);
        } 
        else if (dist >= _enemyData.AttackRange)
        {
            _agent.isStopped = false;
            _enemyController.GetNavAgentRef().SetDestination(_enemyController.CurrentTarget.position);
        }
    }

    public override void OnEnter(EnemyController enemyController) 
    {
        _enemyController = enemyController;
        _enemyData = _enemyController.GetEnemyDataRef();
        _animator = _enemyController.GetAnimatorRef();
        _animator.SetBool("isWalking", false);
        _agent = _enemyController.GetNavAgentRef();
        _agent.isStopped = true;
        AttackTarget();
    }
    
    public override void OnUpdate()
    {
        
    }

    public override void OnExit() { }
}
