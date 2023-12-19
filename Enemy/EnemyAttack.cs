using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;

    public void AttackTarget()
    {
        _enemyAI.DealDamageToTarget();
    }
}

// public class RangedAttack : EnemyAttack
// {
//     [SerializeField] private EnemyAttackSO _attackSO;
//     [SerializeField] private GameObject _projectile;
//     private GameObject _target;

//     public void AttackTarget(GameObject target)
//     {
//         _target = target;
//         float dist = Vector3.Distance(target.transform.position, transform.position);

//         if (dist <= _attackSO.AttackRange) {
//             // _isAttacking = true;
//             _animator.SetTrigger("attackTrigger");
//             Invoke("AttackTarget", _attackSO.AttackCooldown);
//         } else {
//             // _isAttacking = false;
//         }
//     }
    
//     public void ShootProjectile()
//     {
//         Instantiate(_projectile, transform.position, Quaternion.identity);
//         EnemyProjectile projectile = _projectile.GetComponent<EnemyProjectile>();
//         projectile.SetDirection(_target.transform.position);
//         projectile.SetInstigator(this);
//     }
// }

// public class MeleeAttack : EnemyAttack
// {
//     [SerializeField] private EnemyAttackSO _attackSO;

//     public void AttackTarget(GameObject target)
//     {
//         float dist = Vector3.Distance(target.transform.position, transform.position);

//         if (dist <= _attackSO.AttackRange) {
//             // _isAttacking = true;
//             _animator.SetTrigger("attackTrigger");
//             Invoke("AttackTarget", _attackSO.AttackCooldown);
//         } else {
//             // _isAttacking = false;
//         }
//     }
// }

// public class HeavyAttack : EnemyAttack
// {
//     [SerializeField] private EnemyAttackSO _attackSO;

//     public void AttackTarget(GameObject target)
//     {

//     }
// }

// public enum EnemyType
// {
//     Melee,
//     Heavy,
//     Ranged
// }
