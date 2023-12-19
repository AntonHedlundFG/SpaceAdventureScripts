using UnityEngine;

public class SimpleGun : BaseGun
{
    
    private void Update()
    {
        if (_isPrimaryFiring && Time.time >= (_lastPrimaryFire + _primarySettings.RateOfFire) && !_health.IsDead)
        {
            ShootPrimaryBullet();
        }
    }

    private void ShootPrimaryBullet()
    {
        GameObject newBullet = Instantiate(_primarySettings.BulletPrefab);
        newBullet.transform.position = _bulletSpawnPoint.position;
        Projectile projectile = newBullet.GetComponent<Projectile>();
        projectile.SetDirection(transform.forward);
        projectile.SetInstigator(gameObject);
        _lastPrimaryFire = Time.time;
        FMODUnity.RuntimeManager.PlayOneShot(_primarySettings.FireSound, GetComponent<Transform>().position);
    }
}
