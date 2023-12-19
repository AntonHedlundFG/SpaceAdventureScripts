using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsPlayer : Action
{
    private NavMeshAgent _agent;
    private EnemyController _enemyController;

    public override void OnEnter(EnemyController enemyController) 
    {
        _enemyController = enemyController;
        _agent = _enemyController.GetNavAgentRef();
        _agent.isStopped = false;
        _enemyController.GetAnimatorRef().SetBool("isWalking", true);
        // Debug.Log("");
    }

    public override void OnUpdate()
    {
        _agent.SetDestination(_enemyController.CurrentTarget.position);
    }

    public override void OnExit() 
    {
        Debug.Log("Exiting MoveTowardsPlayer");
    }
}
