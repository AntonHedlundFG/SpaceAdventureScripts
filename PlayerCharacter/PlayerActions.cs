using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerActions : MonoBehaviour
{
    private PlayerInput _input;
    private InputAction _useAction;
    
    [SerializeField] private PlayerSettings _settings;

    private ResourceController _usingRC;
    private PlayerUseButton _usingBtn;

    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        _useAction = _input.actions.FindAction("Use");

        _useAction.started += OnUseStart;
        _useAction.canceled += OnUseEnd;
    }

    public void OnUseStart(InputAction.CallbackContext context)
    {
        ResourceController rc;
        PlayerUseButton btn;
        if (!FindNearbyResource(out rc, out btn))
        {
            return;
        }

        if (rc != null)
        {
            _usingRC = rc;
            _usingRC.BeginCollectResource(this);
        }

        if (btn != null)
        {
            _usingBtn = btn;
            btn.PressButton();
        }
    }

    private void OnUseEnd(InputAction.CallbackContext context)
    {
        _usingRC?.StopCollectResource(this);
        _usingRC = null;

        _usingBtn?.StopPressButton();
        _usingBtn = null;
    }

    private bool FindNearbyResource(out ResourceController rc, out PlayerUseButton btn)
    {
        Collider[] resources = Physics.OverlapSphere(transform.position, _settings.ResourceUseRadius, _settings.ResourceLayerMask);
        if (resources.Length == 0)
        {
            rc = null;
            btn = null;
            return false;
        }

        if (resources[0].TryGetComponent<ResourceController>(out rc))
        {
            btn = null;
            return true;
        }

        if (resources[0].TryGetComponent<PlayerUseButton>(out btn))
        {
            rc = null;
            return true;
        }

        return false;
    }
}
