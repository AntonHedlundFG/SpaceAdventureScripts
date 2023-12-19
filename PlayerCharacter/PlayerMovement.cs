using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _eyes;
    [SerializeField] private PlayerSettings _settings;
    [SerializeField] private Transform _gun;
    [SerializeField] private PlayerHealth _health;
    [SerializeField] private Animator _animator;

    [SerializeField] private bool _fallImmediatelyOnCeilingCollision = true;

    [SerializeField] private LayerMask _groundMask;
    [SerializeField][Range(0f, 1f)] private float _slopeLimit = 0.5f;

    private CharacterController _controller;
    private PlayerInput _input;
    private InputAction _moveAction;
    private InputAction _xAction;
    private InputAction _yAction;
    private float animSpeed;

    private bool IsDead = false;
    private bool _hasJumped = false;

    private Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInput>();
        _moveAction = _input.actions.FindAction("HorizontalMovement");
        _xAction = _input.actions.FindAction("MouseX");
        _yAction = _input.actions.FindAction("MouseY");

        Mathf.Clamp01(animSpeed);
    }

    private void OnEnable()
    {
        _health?.OnPlayerDeath.AddListener(SetDead);
    }

    private void OnDisable()
    {
        _health?.OnPlayerDeath.RemoveListener(SetDead);

    }

    private void Update()
    {
        ApplyGravity();
        ApplyHorizontalMovement();
        ApplyDrag();
        SetWalkAnim();
        
        CollisionFlags flags = _controller.Move(_velocity * Time.deltaTime);
        CheckCeiling(flags);
        
        PerformXRotation();
        PerformYRotation();
    }

    private void CheckCeiling(CollisionFlags flags)
    {
        if (_fallImmediatelyOnCeilingCollision && _hasJumped && (flags & CollisionFlags.Above) != 0)
        {
            _hasJumped = false;
            _velocity.y = 0;
        }
    }

    private void SetWalkAnim()
    {
        if (IsDead)
        {
            _animator.SetFloat("speed", 0f);
            return;
        }

        Vector2 input = _moveAction.ReadValue<Vector2>();
        input.x = Mathf.Abs(input.x);
        input.y = Mathf.Abs(input.y);
        float speed = Mathf.Max(input.x, input.y);
        if (speed < 0.1f)
            speed = 0;

        _animator.SetFloat("speed", speed);
    }

    public void SetDead(bool status) => IsDead = status;

    public void OnJump()
    {
        if (_controller.isGrounded && _health.Health > 0 && CheckGroundNormal())
        {
            _velocity.y = _settings.JumpPower;
            _hasJumped = true;
        }
    }

    private bool CheckGroundNormal()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 3f, _groundMask))
        {
            if (hit.normal.normalized.y < _slopeLimit)
            {
                return false;
            }
        }
        return true;
    }

    private void ApplyGravity()
    {
        if (!_controller.isGrounded)
        {
            _velocity.y -= _settings.Gravity * Time.deltaTime;
        }
    }

    private void ApplyHorizontalMovement()
    {
        Vector2 input = _moveAction.ReadValue<Vector2>();
        Vector3 horizontalMove = new Vector3(input.x, 0, input.y);
        horizontalMove = Quaternion.Euler(0, transform.eulerAngles.y, 0) * horizontalMove;
        if (IsDead)
        {
            horizontalMove *= _settings.DyingSpeedMultiplier;
        }
        _velocity.x += horizontalMove.x * _settings.MovementSpeed * Time.deltaTime;
        _velocity.z += horizontalMove.z * _settings.MovementSpeed * Time.deltaTime;
    }

    private void ApplyDrag()
    {
        float drag = 1f - _settings.DragMultiplier * Time.deltaTime;
        _velocity.x *= drag;
        _velocity.z *= drag;
    }

    private void PerformXRotation()
    {
        float xRotate = _xAction.ReadValue<float>();
        float rotation = _settings.XRotateSpeed * Time.deltaTime * xRotate;
        if (IsDead)
        {
            rotation *= _settings.DyingAimSpeedMultiplier;
        }
        transform.Rotate(Vector3.up, rotation);
    }

    private void PerformYRotation()
    {
        float yRotate = _yAction.ReadValue<float>();
        float rotateVal = -yRotate * Time.deltaTime * _settings.YRotateSpeed;
        if (IsDead)
        {
            rotateVal *= _settings.DyingAimSpeedMultiplier;
        }
        float targetRotation = _eyes.localEulerAngles.x + rotateVal;
        if (targetRotation > 180f)
        {
            targetRotation -= 360f;

        }
        if (targetRotation < _settings.MinYRotation || targetRotation > _settings.MaxYRotation)
        {
            return;
        }
        _eyes.localEulerAngles += new Vector3(rotateVal, 0f, 0f);
        _gun.localEulerAngles += new Vector3(rotateVal, 0f, 0f);
    }
}
