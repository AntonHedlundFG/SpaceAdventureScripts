using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttack", menuName = "My Game/Enemy Attack Data")]
public class EnemyAttackSO : ScriptableObject
{
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _damage = 1;
    [SerializeField, Range(0,100)] private int _slowAmountInPercent = 50;


    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;
    public int Damage => _damage;
    public int SlowAmountInPercent => _slowAmountInPercent;
}
