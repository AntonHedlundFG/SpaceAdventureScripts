using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class BaseGun : MonoBehaviour
{
    [SerializeField] protected Transform _bulletSpawnPoint;
    [SerializeField] protected HealthComponent _health;
    [SerializeField] protected WeaponSettings _primarySettings;
    [SerializeField] protected WeaponSettings _secondarySettings;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected AnimationClip _primaryAnimation;

    protected PlayerInput _input;
    protected InputAction _primaryFireAction;
    protected InputAction _secondaryFireAction;

    protected bool _isPrimaryFiring = false;
    protected float _lastPrimaryFire;

    protected bool _isSecondaryFiring = false;
    protected float _lastSecondaryFire;

    public UnityEvent<float> OnPrimaryCooldownStarted;
    public UnityEvent<float> OnSecondaryCooldownStarted;

    protected virtual void Start()
    {
        // _animator.SetFloat("primarySpeed", _primaryAnimation.length / _primarySettings.RateOfFire);
        _animator.speed = _primaryAnimation.length / _primarySettings.RateOfFire;
        _input = GetComponentInParent<PlayerInput>();
        _primaryFireAction = _input.actions.FindAction("PrimaryFire");
        _secondaryFireAction = _input.actions.FindAction("SecondaryFire");

        _primaryFireAction.started += OnPrimaryFireStart;
        _primaryFireAction.canceled += OnPrimaryFireEnd;
        _secondaryFireAction.started += OnSecondaryFireStart;
        _secondaryFireAction.canceled += OnSecondaryFireEnd;

        _lastPrimaryFire = Time.time - _primarySettings.RateOfFire;
        _lastSecondaryFire = Time.time - _secondarySettings.RateOfFire;

        if (_health == null)
        {
            _health = GetComponentInParent<HealthComponent>();
        }

    }

    protected virtual void OnPrimaryFireStart(InputAction.CallbackContext context)
    {
        _isPrimaryFiring = true;
    }

    protected virtual void OnPrimaryFireEnd(InputAction.CallbackContext context)
    {
        _isPrimaryFiring = false;
    }

    protected virtual void OnSecondaryFireStart(InputAction.CallbackContext context)
    {
        _isSecondaryFiring = true;
    }

    protected virtual void OnSecondaryFireEnd(InputAction.CallbackContext context)
    {
        _isSecondaryFiring = false;
    }
}
