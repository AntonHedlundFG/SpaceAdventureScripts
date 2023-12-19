using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : Action
{
    private Animator _animator;

    public override void OnUpdate()
    {

    }

    public override void OnEnter(EnemyController enemyController) 
    {
        _animator = enemyController.GetAnimatorRef();
        _animator.SetBool("isWalking", false);
    }

    public override void OnExit() 
    {
        _animator.SetBool("isWalking", true);
    }
}
