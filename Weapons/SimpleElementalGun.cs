using UnityEngine;

public class SimpleElementalGun : BaseGun
{
    [SerializeField] private EventSO _activateSecondaryModeEvent;
    private bool _secondaryModeActive = true;

    [SerializeField] private Camera _playerCamera;
    [SerializeField] private LayerMask _aimMask;
    [SerializeField][Range(0f, 2f)] private float _lowerAimThreshold = 1f;

    [SerializeField] private GameObject _muzzleFlashPrefab;
    [SerializeField] private string _muzzleFlashLayer;

    private void OnEnable()
    {
        if (_activateSecondaryModeEvent != null)
        {
            _secondaryModeActive = false;
            _activateSecondaryModeEvent.Event.AddListener(ActivateSecondaryWeapon);
        }
    }

    private void OnDisable()
    {
        if (_activateSecondaryModeEvent != null)
        {
            _activateSecondaryModeEvent.Event.RemoveListener(ActivateSecondaryWeapon);
        }
    }
    private void Update()
    {
        if (_isPrimaryFiring 
            && Time.time >= (_lastPrimaryFire + _primarySettings.RateOfFire) 
            && !_health.IsDead
            && Time.timeScale > 0f)
        {
            ShootPrimaryBullet();
        }

        if (_secondaryModeActive 
            && _isSecondaryFiring 
            && Time.time >= (_lastSecondaryFire + _secondarySettings.RateOfFire) 
            && !_health.IsDead
            && Time.timeScale > 0f)
        {
            ShootSecondaryBullet();
        }
    }

    public void ActivateSecondaryWeapon()
    {
        _secondaryModeActive = true;
    }

    private void ShootPrimaryBullet()
    {
        _animator.SetTrigger("attackTrigger");
        GameObject newBullet = Instantiate(_primarySettings.BulletPrefab);
        newBullet.transform.position = _bulletSpawnPoint.position;
        Projectile projectile = newBullet.GetComponent<Projectile>();

        Vector3 projDirection = _playerCamera != null ? GetCameraAim(_playerCamera) : transform.forward;
        projectile.SetDirection(projDirection);

        projectile.SetInstigator(transform.parent.gameObject);
        _lastPrimaryFire = Time.time;
        OnPrimaryCooldownStarted.Invoke(_primarySettings.RateOfFire);
        FMODUnity.RuntimeManager.PlayOneShot(_primarySettings.FireSound, GetComponent<Transform>().position);

        MuzzleFlash();
    }

    private void ShootSecondaryBullet()
    {
        _animator.SetTrigger("secondaryTrigger");
        GameObject newBullet = Instantiate(_secondarySettings.BulletPrefab);
        newBullet.transform.position = _bulletSpawnPoint.position;
        Projectile projectile = newBullet.GetComponent<Projectile>();
        projectile.SetDirection(transform.forward);
        projectile.SetInstigator(transform.parent.gameObject);
        _lastSecondaryFire = Time.time;
        OnSecondaryCooldownStarted.Invoke(_secondarySettings.RateOfFire);
        FMODUnity.RuntimeManager.PlayOneShot(_secondarySettings.FireSound, GetComponent<Transform>().position);
    }

    private void MuzzleFlash()
    {
        if (_muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(_muzzleFlashPrefab, _bulletSpawnPoint.position, Quaternion.identity, transform);
            flash.transform.forward = transform.forward;
            flash.layer = gameObject.layer;
        }
    }

    private Vector3 GetCameraAim(Camera camera)
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 200f, _aimMask))
        {
            Vector3 aimVector = hitInfo.point - _bulletSpawnPoint.position;
            if (aimVector.magnitude >= _lowerAimThreshold)
            {
                return aimVector.normalized;
            }
        }
        return transform.forward;
    }
}
