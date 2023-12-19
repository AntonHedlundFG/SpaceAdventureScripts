using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTowardsMothership : Action
{
    private NavMeshAgent _agent;
    private EnemyController _enemyController;

    public override void OnEnter(EnemyController enemyController) 
    {
        _agent = enemyController.GetNavAgentRef();
        _enemyController = enemyController;
        _agent.isStopped = false;
        _enemyController.CurrentTarget = _enemyController.GetMothershipRef();
        _enemyController.GetAnimatorRef().SetBool("isWalking", true);
    }

    public override void OnUpdate()
    {
        _agent.SetDestination(_enemyController.CurrentTarget.position);
    }

    public override void OnExit() { }
}
