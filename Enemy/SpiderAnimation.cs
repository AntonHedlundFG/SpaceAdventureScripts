using System.Collections;
using UnityEngine;

public class SpiderAnimation : MonoBehaviour
{
    [SerializeField] private Transform[] _limbTargets;
    [SerializeField] private float _stepSize = 1;
    [SerializeField] private LayerMask _groundLayerMask = default;
    [SerializeField] private float _raycastRange = 2;
    [SerializeField] private Transform _headTransform;
    [SerializeField] private string _walkSound;

    private int _nLimbs;
    private ProceduralLimb[] _limbs;

    // Head Tilt
    private Vector3 _lastBodyPosition;
    private Vector3 _velocity;
    private float _angle = -90f;
    private float _signedAng;
    private float _resetTimer = 0;
    [SerializeField] private float rotationMultiplier = 50;
    [SerializeField] private float _headRotationSpeed = 5;
    [SerializeField] private float _resetSpeed = 1;
    [SerializeField] private float _headRotationMin = -75;
    [SerializeField] private float _headRotationMax = -105;
    [SerializeField] private float _resetThreshold = 0.5f;
    private Vector3 defaultAngle = new Vector3(-90f, 0, -180f);

    // Leg movement
    [SerializeField] private float _stepHeight = 1;
    [SerializeField] private int _smoothness = 1;
    [SerializeField] private float _feetOffset = 0;

    void Start()
    {
        _nLimbs = _limbTargets.Length;
        _limbs = new ProceduralLimb[_nLimbs];
        Transform t;
        for (int i = 0; i < _nLimbs; ++i)
        {
            t = _limbTargets[i];
            _limbs[i] = new ProceduralLimb()
            {
                IKTarget = t,
                defaultPosition = t.localPosition,
                lastPosition = t.position
            };
        }
        // _allLimbsResting = true;
        _headTransform.localRotation = Quaternion.Euler(-90, 0, -180);
    }

    void FixedUpdate()
    {
        _velocity = transform.position - _lastBodyPosition;
        _resetTimer += Time.deltaTime;
        

        if (_velocity.magnitude > Mathf.Epsilon)
        {
            _resetTimer = 0;
            HandleMovement();
            TiltHead();
        }
        else if (_resetTimer >= _resetThreshold)
        {
            BackToRestPosition();
            // ResetHead();
        }
    }

    private void TiltHead()
    {
        _signedAng = Mathf.Sign(_velocity.z);
        _angle += _signedAng * rotationMultiplier * Time.deltaTime;
        _angle = Mathf.Clamp(_angle, _headRotationMax, _headRotationMin);
        float targetAngle = _angle;
        float currentAngle = _headTransform.localEulerAngles.x;
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Mathf.SmoothStep(0, 1, _headRotationSpeed));
        _headTransform.localEulerAngles = new Vector3(currentAngle, 0, -180);
    }

    private void ResetHead()
    {
        float currentAngle = _headTransform.localEulerAngles.x - 360;
        float currentAngles = _headTransform.localRotation.x;
        // currentAngle = Mathf.Clamp(currentAngle, _headRotationMin, _headRotationMax);
        // float currentAngle = _headTransform.local;
        Debug.Log(currentAngles);
        float targetAngle = -90;
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * _resetSpeed);
        _headTransform.localEulerAngles = new Vector3(currentAngle, 0, -180);
    }

    private Vector3 RaycastToGround(Vector3 pos, Vector3 up)
    {
        Vector3 point = pos;

        Ray ray = new Ray(pos + _raycastRange * up, -up);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f * _raycastRange, _groundLayerMask))
            point = hit.point;
        return point;
    }

    private void HandleMovement()
    {
        _velocity = transform.position - _lastBodyPosition;
        _lastBodyPosition = transform.position;
        
        Vector3[] desiredPositions = new Vector3[_nLimbs];
        float greatestDistance = _stepSize;
        int limbToMove = -1;

        for (int i = 0; i < _nLimbs; ++i)
        {
            if (_limbs[i].moving) continue; // limb already moving: can't move again!

            desiredPositions[i] = transform.TransformPoint(_limbs[i].defaultPosition);
            float dist = (desiredPositions[i] + _velocity - _limbs[i].lastPosition).magnitude;
            if (dist > greatestDistance)
            {
                greatestDistance = dist;
                limbToMove = i;
            }
        }

        // keep non moving limbs in place
        for (int i = 0; i < _nLimbs; ++i)
            if (i != limbToMove)
                _limbs[i].IKTarget.position = _limbs[i].lastPosition;

        // move the selected leg to its "desired" position
        if (limbToMove != -1)
        {
            Vector3 targetOffset = desiredPositions[limbToMove] - _limbs[limbToMove].IKTarget.position;
            Vector3 targetPoint = desiredPositions[limbToMove] + _velocity.magnitude * targetOffset;
            targetPoint = RaycastToGround(targetPoint, transform.up);
            targetPoint += transform.up * _feetOffset;

            
            StartCoroutine(Stepping(limbToMove, targetPoint));
        }
    }

    private void BackToRestPosition()
    {
        Vector3 targetPoint; float dist;
        for (int i = 0; i < _nLimbs; ++i)
        {
            if (_limbs[i].moving) continue; // limb already moving: can't move again!

            targetPoint = RaycastToGround(
                transform.TransformPoint(_limbs[i].defaultPosition),
                transform.up) + transform.up * _feetOffset;
            dist = (targetPoint - _limbs[i].lastPosition).magnitude;
            if (dist > 0.005f)
            {
                StartCoroutine(Stepping(i, targetPoint));
                return;
            }
        }
        // _allLimbsResting = true;
    }

    private IEnumerator Stepping(int limbIdx, Vector3 targetPosition)
    {
        _limbs[limbIdx].moving = true;
        Vector3 startPosition = _limbs[limbIdx].lastPosition;
        float t;
        for (int i = 1; i <= _smoothness; ++i)
        {
            t = i / (_smoothness + 1f);
            _limbs[limbIdx].IKTarget.position =
                Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, t))
                + transform.up * Mathf.Sin(t * Mathf.PI) * _stepHeight;
            yield return new WaitForFixedUpdate();
        }

        if (_walkSound != "")
        {
            FMODUnity.RuntimeManager.PlayOneShot(_walkSound);
        }
        _limbs[limbIdx].IKTarget.position = targetPosition;
        _limbs[limbIdx].lastPosition = targetPosition;
        _limbs[limbIdx].moving = false;
    }

    private IEnumerator SmoothStepping(int limbIdx, Vector3 targetPosition)
{
    _limbs[limbIdx].moving = true;
    Vector3 startPosition = _limbs[limbIdx].lastPosition;
    float t;
    int counter = 0;

    for (int i = 1; i <= _smoothness; ++i)
    {
        t = i / (_smoothness + 1f);
        if (counter % 2 == limbIdx % 2)
        {
            _limbs[limbIdx].IKTarget.position =
                Vector3.Lerp(startPosition, targetPosition, Mathf.SmoothStep(0, 1, t))
                + transform.up * Mathf.Sin(t * Mathf.PI) * _stepHeight;
        }
        counter++;
        yield return new WaitForFixedUpdate();
    }
    _limbs[limbIdx].IKTarget.position = targetPosition;
    _limbs[limbIdx].lastPosition = targetPosition;
    _limbs[limbIdx].moving = false;
}
}

class ProceduralLimb
{
    public bool moving;
    public Transform IKTarget;
    public Vector3 defaultPosition;
    public Vector3 lastPosition;
}
