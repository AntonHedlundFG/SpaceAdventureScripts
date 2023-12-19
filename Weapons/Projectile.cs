using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 _directionVector;
    protected GameObject _instigator; // The player that shoots the projectile
    protected Rigidbody _rb;

    [SerializeField] protected WeaponSettings _settings;

    [SerializeField] protected GameObject _impactEffectPrefab;

    private float _spawnTime;

    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        if (_rb != null)
        {
            _rb.useGravity = _settings.AffectedByGravity;
        }
    }
    protected virtual void Awake()
    {
        _spawnTime = Time.time;
    }
    protected virtual void Update()
    {
        if (Time.time > _spawnTime + _settings.Duration) {
            Destroy(gameObject);
        }
    }

    protected void FixedUpdate()
    {
        transform.Translate(_directionVector * (Time.fixedDeltaTime * _settings.ProjectileSpeed), Space.World);
    }

    public void SetDirection(Vector3 vec)
    {
        _directionVector = vec.normalized;
        transform.forward = _directionVector;
    }

    public virtual void OnTriggerEnter(Collider other)
    {

        EnemyHealth eh;
        if (other.TryGetComponent<EnemyHealth>(out eh))
        {
            eh.DealDamage(_settings.BaseDamage, _instigator);
            
        }
        Destroy(gameObject);
    }

    protected void CreateImpactEffect()
    {
        if (_impactEffectPrefab != null)
        {
            Instantiate(_impactEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    public void SetInstigator(GameObject player)
    {
        _instigator = player;
    }
}
