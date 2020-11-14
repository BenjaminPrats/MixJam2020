using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody _rb;
    public Transform _cameraTarget;
    public Transform _rayGroundCheckPoint;
    public Transform _raySlopeCheckPoint;
    public LayerMask _groundMask;
    public BoostBar _boostBar;

    [Space(10)]
    public float _accelerationFactor = 10.0f;
    public float _forwardAcceleration = 10.0f;
    public float _reverseAcceleration = 5.0f;

    [Space(10)]
    public float _boostFactor = 1.0f;
    public float _boostFactorInAir = 1.0f;
    public float _boostDuration = 5.0f; // if 5 means take 5 seconds to empty it fully 
    public float _boostCameraDelay = 5.0f;

    [Space(10)]
    public float _rotationStrength = 5.0f;
    public float _cameraRotationRadius = 10.0f;
    public float _cameraSmoothTime = 0.3f;

    [Space(10)]
    public float _smoothNormalRotationMax = 10.0f;
    public float _lengthNormalRotationCheck = 5.0f;
    //    public float _maxSpeed = 10.0f;

    [Space(10)]
    public float _gravityStrength = 10.0f;
    public float _groundRayCheckLength = 0.5f;
    public float _dragGround = 1.5f;
    public float _dragAir = 0.5f;

    [Space(10)]
    [Range(0.0f, 90.0f)]
    public float _maxSlope = 70.0f;


    private float _accelerationInput;
    private float _rotationInput;
    private float _verticalInput;

    private bool _isGrounded = false;
    private bool _isBoostActivated = false;

    private float _boostLevel = 0.0f; // 0 to 1

    private Vector3 _cameraFollowVelocity = Vector3.zero;

    private Quaternion _futureRotation;

    // Start is called before the first frame update
    void Start()
    {
        _rb.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Inputs
        _verticalInput = Input.GetAxis("Vertical");
        _rotationInput = Input.GetAxis("Horizontal");

        _accelerationInput = _verticalInput > 0.0f ? _verticalInput * _forwardAcceleration :
                             _verticalInput < 0.0f ? _verticalInput * _reverseAcceleration : 0.0f;

        _boostBar.SetSlider(_boostLevel);
        _isBoostActivated = (Input.GetButton("Jump") && _boostLevel > 0.0f) ? true : false;

        // Camera movement
        Vector3 cameraTargetTarget = new Vector3(_rotationInput * _cameraRotationRadius, _cameraTarget.localPosition.y, 0.0f);
        float cameraSmoothTime = _cameraSmoothTime;
        if (_isBoostActivated)
        {
            cameraTargetTarget.z = -_boostCameraDelay;
            cameraSmoothTime = 0.5f;
        }
        _cameraTarget.localPosition = Vector3.SmoothDamp(_cameraTarget.localPosition, cameraTargetTarget, ref _cameraFollowVelocity, _cameraSmoothTime);

        // Update car transform
//        UpdateCarRotation();
        if (_isGrounded || _isBoostActivated)
        {
            float verticalFactor = _verticalInput == 0.0f && (_isBoostActivated || (_isGrounded && _rb.velocity.sqrMagnitude > 1.0f)) ? Mathf.Clamp01(_rb.velocity.sqrMagnitude) : _verticalInput;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f, _rotationInput * _rotationStrength * Time.deltaTime * verticalFactor, 0.0f));
        }

        transform.position = _rb.transform.position;
    }

    private void FixedUpdate()
    {
        bool isGroundedOld = _isGrounded;
        RaycastHit hit;
        if (Physics.Raycast(_rayGroundCheckPoint.position, -transform.up, out hit, _groundRayCheckLength, _groundMask))
        {
            //// car perpendicular to road
            //Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _smoothNormalRotation);

            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        UpdateCarRotation();

        if (isGroundedOld != _isGrounded)
        {
            _rb.drag = _isGrounded ? _dragGround : _dragAir;
        }

        if (_isGrounded)
        {
            if (_accelerationInput != 0.0f)
                _rb.AddForce(transform.forward * _accelerationInput * _accelerationFactor);

            //float dot = Vector3.Dot(transform.up, Vector3.up);
            //if (dot < 0.7f)
            //{
            //    float slopeDecelerationFactor = Mathf.Clamp(1.0f / (dot * dot), 0.0f, 20.0f) / 10.0f;
            //    Debug.Log("Deceleration factor: " + slopeDecelerationFactor);
            //    _rb.AddForce(-transform.forward * _accelerationFactor * slopeDecelerationFactor * _accelerationFactor);
            //}

        }
        else
        {
            if (_rb.velocity.y < 0.0f)
                _rb.AddForce(Vector3.down * _gravityStrength);
        }

        if (_isBoostActivated)
        {
            float boostFactor = _isGrounded ? _boostFactor : _boostFactorInAir;
            _rb.AddForce(transform.forward * boostFactor * _accelerationFactor);
            _boostLevel -= Time.fixedDeltaTime * 1.0f / _boostDuration;
            if (_boostLevel < 0.0f)
                _boostLevel = 0.0f;
        }
    }

    private void UpdateCarRotation()
    {
        RaycastHit hit;
        if (Physics.Raycast(_rayGroundCheckPoint.position, -transform.up, out hit, _lengthNormalRotationCheck, _groundMask))
        {
            float smoothnessFactor = 1.0f / (hit.distance / _lengthNormalRotationCheck);
            if (smoothnessFactor > _smoothNormalRotationMax)
                smoothnessFactor = _smoothNormalRotationMax;

            float slopePercent = 1.0f - Mathf.Abs(Vector3.Dot(hit.normal.normalized, Vector3.up));
            if (slopePercent < 0.4f)
            {
                // car perpendicular to road
                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, smoothnessFactor);
            }
        }

    }

    public void ReloadBoost(float amount)
    {
        _boostLevel += amount;
        if (_boostLevel > 1.0f)
            _boostLevel = 1.0f;
    }

    public float GetVerticalInput()
    {
        return _verticalInput;
    }

    public float GetRotationInput()
    {
        return _rotationInput;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rayGroundCheckPoint.position, _rayGroundCheckPoint.position - (transform.up * _groundRayCheckLength));
    }
}
