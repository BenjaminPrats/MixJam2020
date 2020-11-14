using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Rigidbody _rb;
    public Transform _cameraTarget;
    public Transform _rayCheckPoint;
    public LayerMask _groundMask;
    public BoostBar _boostBar;

    public float _forwardAcceleration = 10.0f;
    public float _reverseAcceleration = 5.0f;

    public float _boostFactor = 1.0f;
    public float _boostDuration = 5.0f; // if 5 means take 5 seconds to empty it fully 
    public float _boostCameraDelay = 5.0f;

    public float _rotationStrength = 5.0f;
    public float _cameraRotationRadius = 10.0f;
    public float _cameraSmoothTime = 0.3f;

    public float _smoothNormalRotation = 10.0f;
    //    public float _maxSpeed = 10.0f;

    public float _gravityStrength = 10.0f;
    public float _groundRayCheckLength = 0.5f;

    private float _accelerationInput;
    private float _rotationInput;

    private bool _isGrounded = false;
    private bool _isBoostActived = false;

    private float _boostLevel = 0.0f; // 0 to 1


    private Vector3 _cameraFollowVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        _rb.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Inputs
        float verticalInput = Input.GetAxis("Vertical");
        _rotationInput = Input.GetAxis("Horizontal");

        _accelerationInput = verticalInput > 0.0f ? verticalInput * _forwardAcceleration :
                             verticalInput < 0.0f ? verticalInput * _reverseAcceleration : 0.0f;

        _boostBar.SetSlider(_boostLevel);
        _isBoostActived = (Input.GetButton("Jump") && _boostLevel > 0.0f) ? true : false;

        //if ()
        //{

        //    _accelerationInput += (_forwardAcceleration * _boostFactor);
        //    _rb.drag = 0.0f;
        //}
        //else
        //{
        //    _rb.drag = 0.5f;
        //}

        // Camera movement
        Vector3 cameraTargetTarget = new Vector3(_rotationInput * _cameraRotationRadius, 0.0f, 0.0f);
        float cameraSmoothTime = _cameraSmoothTime;
        if (_isBoostActived)
        {
            cameraTargetTarget.z = -_boostCameraDelay;
            cameraSmoothTime = 0.5f;
        }
        _cameraTarget.localPosition = Vector3.SmoothDamp(_cameraTarget.localPosition, cameraTargetTarget, ref _cameraFollowVelocity, _cameraSmoothTime);

        // Update car transform
        if (_isGrounded || _isBoostActived)
        {
            float verticalFactor = verticalInput < 0.01f && (_isBoostActived || _rb.velocity.sqrMagnitude > 1.0f) ? 1.0f : verticalInput; 
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f, _rotationInput * _rotationStrength * Time.deltaTime * verticalFactor, 0.0f));
        }


        transform.position = _rb.transform.position;
    }

    private void FixedUpdate()
    {
        _isGrounded = false;

        RaycastHit hit;
        if (Physics.Raycast(_rayCheckPoint.position, -transform.up, out hit, _groundRayCheckLength, _groundMask))
        {
            // car perpendicular to road
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _smoothNormalRotation);

            _isGrounded = true;
        }

        if (_isGrounded)
        {
            if (_accelerationInput != 0.0f)
                _rb.AddForce(transform.forward * _accelerationInput);
        }
        else
        {
            if (_rb.velocity.y < 0.0f)
                _rb.AddForce(Vector3.down * _gravityStrength);
        }

        if (_isBoostActived)
        {
            _boostLevel -= Time.fixedDeltaTime * 1.0f / _boostDuration;
            if (_boostLevel < 0.0f)
                _boostLevel = 0.0f;
            _rb.AddForce(transform.forward * _boostFactor);
        }
    }

    public void ReloadBoost(float amount)
    {
        _boostLevel += amount;
        if (_boostLevel > 1.0f)
            _boostLevel = 1.0f;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_rayCheckPoint.position, _rayCheckPoint.position - (transform.up * _groundRayCheckLength));
    }
}
