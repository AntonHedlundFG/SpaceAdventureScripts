using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class PassiveHealthRegen : MonoBehaviour
{
    private HealthComponent _hc;
    private PlayerHealth _health;

    [SerializeField] private float _timeBeforeRegenStarts = 5f;
    [SerializeField] private float _secondsPerHP = 1f;

    private int _lastKnownHP;
    private Coroutine _regenRoutine;

    private void Start()
    {
        _hc = GetComponent<HealthComponent>();
        _health = _hc.GetPlayerHealth();
        _health.OnPlayerHealthChange.AddListener(HealthChanges);
        _lastKnownHP = _health.Health;
    }

    private void HealthChanges(int newHP)
    {
        if (_lastKnownHP > newHP)
        {
            DamageTaken();
        }
        if (_lastKnownHP <= 0 && newHP > 0)
        {
            DamageTaken();
        }
        _lastKnownHP = newHP;
    }

    private void DamageTaken()
    {
        if (_regenRoutine != null) {StopCoroutine(_regenRoutine);}

        _regenRoutine = StartCoroutine(RegenRoutine());

    }

    private IEnumerator RegenRoutine()
    {
        yield return new WaitForSeconds(_timeBeforeRegenStarts);
        while (_health.Health < _health.maxHealth && _health.Health > 0)
        {
            _health.Health++;
            yield return new WaitForSeconds(_secondsPerHP);
        }
    }

}
