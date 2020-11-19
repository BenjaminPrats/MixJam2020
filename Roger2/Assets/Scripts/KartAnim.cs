using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartAnim : MonoBehaviour
{
    public CarController carController;

    public Transform _wheelsBack;
    public Transform _wheelFront_Right;
    public Transform _wheelFront_Left;
    public Transform _barFront;
    public Transform _steeringWheel;

    public float _maxRotationWheel = 45.0f;
    public float _maxRotationSteeringWheel = 80.0f;
    public float _smoothRotationFactor = 5.0f;
    public float _wheelSpeed = 1.0f;

    void Update()
    {
        float verticalInput = carController.GetVerticalInput();
        float rotationInput = carController.GetRotationInput();

        Quaternion targetRotationWheel = Quaternion.Euler(0.0f, 0.0f, rotationInput * _maxRotationWheel); // Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        Quaternion smoothRotation = Quaternion.RotateTowards(_wheelFront_Right.localRotation, targetRotationWheel, _smoothRotationFactor);
        _wheelFront_Right.localRotation = smoothRotation;
        _wheelFront_Left.localRotation = smoothRotation;

        _steeringWheel.localRotation = Quaternion.RotateTowards(_steeringWheel.localRotation, Quaternion.Euler(0.0f, 0.0f, -rotationInput * _maxRotationSteeringWheel), _smoothRotationFactor);


        _wheelsBack.localRotation = Quaternion.Euler(_wheelsBack.localRotation.eulerAngles + new Vector3(verticalInput * _wheelSpeed, 0.0f, 0.0f));
    }
}
