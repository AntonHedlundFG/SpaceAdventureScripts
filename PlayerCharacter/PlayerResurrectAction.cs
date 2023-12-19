using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerResurrectAction : MonoBehaviour
{
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private float _duration;

    private PlayerInput _input;
    private InputAction _useAction;

    private LayerMask _playerMask;

    private Coroutine _resurrectRoutine;

    public float resPercentage;

    private bool isRessing;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        _useAction = _input.actions.FindAction("Use");

        _useAction.started += OnUseStart;
        _useAction.canceled += OnUseEnd;

        _playerMask = LayerMask.GetMask(new string[] { "Player" });
    }

    public void OnUseStart(InputAction.CallbackContext context)
    {
        HealthComponent hc;
        if (FindNearbyPlayer(out hc))
        {
            isRessing = true;
            _resurrectRoutine = StartCoroutine(Resurrect(hc));
        }
    }

    private void OnUseEnd(InputAction.CallbackContext context)
    {
        if (_resurrectRoutine != null)
        {
            isRessing = false;
            StopCoroutine(_resurrectRoutine);
        }
    }

    private IEnumerator Resurrect(HealthComponent hc)
    {
        
        for (float i = _duration; i > 0f; i -= 0.1f)
        {
            isRessing = true;
            yield return new WaitForSeconds(0.1f);
            if (Vector3.Distance(transform.position, hc.transform.position) > _settings.ResourceUseRadius)
            {
                isRessing = false;
                resPercentage = 0f;
                yield break;
            }
        }
        hc.Resurrect();
        isRessing = false;
    }

    private bool FindNearbyPlayer(out HealthComponent hc)
    {
        Collider[] players = Physics.OverlapSphere(transform.position, _settings.ResourceUseRadius, _playerMask);

        foreach (Collider cld in players)
        {
            if (cld.TryGetComponent<HealthComponent>(out hc) && hc.gameObject != gameObject && hc.IsDead)
            {
                return true;
            }
        }

        hc = null;
        return false;
    }
    void Update()
    {
        if(isRessing)
        {
            resPercentage += Time.deltaTime / _duration;
        }else
        {
            resPercentage = 0f;
        }
    }
}
