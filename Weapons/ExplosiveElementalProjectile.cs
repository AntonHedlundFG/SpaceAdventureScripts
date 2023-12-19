using UnityEngine;
using Elemental;

public class ExplosiveElementalProjectile : ElementalProjectile
{
    [SerializeField] private float _scaleDif = 0.6f;
    [SerializeField] private float _scaleSpeed = 0.1f;
    private bool _sizeIncreasing = true;
    private Vector3 _startingScale;
    private float _curScale = 1f;
    protected override void Start()
    {
        base.Start();
        _startingScale = transform.localScale;
    }
    protected override void Update()
    {
        base.Update();
        ResizeBlob();
    }

    private void ResizeBlob()
    {
        if (_sizeIncreasing)
        {
            _curScale += Time.deltaTime / _scaleSpeed;
            transform.localScale = _startingScale * _curScale;
            if (_curScale >= 1 + _scaleDif)
            {
                _sizeIncreasing = false;
            }
        }
        else
        {
            _curScale -= Time.deltaTime / _scaleSpeed;
            transform.localScale = _startingScale * _curScale;
            if (_curScale <= 1 - _scaleDif)
            {
                _sizeIncreasing = true;
            }
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _settings.Radius); //LayerMask.GetMask(new string[]{"Enemy"}
        foreach (Collider collider in colliders)
        {
            ElementalTarget target = collider.GetComponentInParent<ElementalTarget>();
            if (target == null)
            {
                continue;
            }
            WeaponHit hit = new WeaponHit(_settings.Type, _instigator, transform.position, _settings.BaseDamage);
            target.ApplyHit(hit);
        }

        SpawnExplosion();

        Destroy(gameObject);
    }

    private void SpawnExplosion()
    {
        if (_settings.Explosion == null)
        {
            return;
        }

        GameObject explosion = Instantiate(_settings.Explosion, transform.position, Quaternion.identity);
        explosion.GetComponent<Explosion>()?.SetRadius(_settings.Radius);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _settings.Radius);
    }

}
