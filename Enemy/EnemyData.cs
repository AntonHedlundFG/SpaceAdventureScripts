using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "My Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string _enemyName;
    [SerializeField] private string _description;
    [SerializeField] private GameObject _enemyModel;
    [SerializeField] private int _health = 20;
    [SerializeField] private float _minMovementSpeed = 2f;
    [SerializeField] private float _maxMovementSpeed = 3f;
    [SerializeField] private float _aggroMovementSpeed = 4f;
    [SerializeField] private float _detectRange = 10f;
    [SerializeField] private float _maxChaseDistance = 30f;
    [SerializeField] private float _outOfBoundsChaseTime = 3f;
    [SerializeField] private float _attackRange = 2f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private int _damage = 1;
    [Range(10,360)] public int _detectionConeDegrees = 90;

    public string Name => _enemyName;
    public string Description => _description;
    public GameObject EnemyModel => _enemyModel;
    public int Health => _health;
    public float MinSpeed => _minMovementSpeed;
    public float MaxSpeed => _maxMovementSpeed;
    public float AggroSpeed => _aggroMovementSpeed;
    public float MaxChaseDist => _maxChaseDistance;
    public float OutOfBoundsChaseTime => _outOfBoundsChaseTime;
    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;
    public float DetectRange => _detectRange;
    public int Damage => _damage;
    public int DetectionConeDegrees => _detectionConeDegrees;

    /*public override void DoTurn()
    {

    }
    */
}
