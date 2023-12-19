using System;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 _directionVector;
    // protected RangedAttack _instigator; // The player that shoots the projectile

    [SerializeField] private float _projectileSpeed = 50f;
    [SerializeField] protected LayerMask _targetLayerMask;
    [SerializeField][Range(0, 50)] protected int _projectileDamage = 1;
    [SerializeField][Range(0f, 10f)] private float _duration = 3f;

    private float _spawnTime;

    private void Awake()
    {
        _spawnTime = Time.time;
    }

    private void Update()
    {
        transform.Translate(_directionVector * (Time.deltaTime * _projectileSpeed));
        if (Time.time > _spawnTime + _duration) {
            Destroy(gameObject);
        }
    }


    public virtual void OnTriggerEnter(Collider other)
    {
        PlayerHealth ph;
        if (other.TryGetComponent<PlayerHealth>(out ph))
        {
            // TODO: SLOW PLAYER        
        }

        Destroy(gameObject);
    }

    public void SetDirection(Vector3 vec)
    {
        _directionVector = vec.normalized;
    }

    // public void SetInstigator(RangedAttack enemy)
    // {
    //     _instigator = enemy;
    // }
}
