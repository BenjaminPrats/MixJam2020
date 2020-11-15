using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTraps : MonoBehaviour
{
    public Vector3 _yTranslation = Vector3.up * 3.0f;
    public float _animTime = 0.5f;

    public bool _isUp = true;

    Vector3 _startingPosition;
    bool _isMoving = false;
    Vector3 _targetPosition;


    // Start is called before the first frame update
    void Start()
    {
        _startingPosition = transform.position;
    }

    private void Update()
    {
        if (_isMoving)
        {
            float sign = (transform.position.y < _targetPosition.y) ? 1.0f : -1.0f;
            Vector3 step = sign * (Time.deltaTime / _animTime) * _yTranslation;

            transform.position += step;

            // ended
            if ((transform.position - _targetPosition).sqrMagnitude < step.y)
            {
                _isMoving = false;
            }
        }
    }

    public void GoUp()
    {
        if (_isUp)
            return;

        _isUp = true;
        _isMoving = true;
        _targetPosition = _startingPosition;
    }

    public void GoDown()
    {
        if (!_isUp)
            return;

        _isUp = false;
        _isMoving = true;
        _targetPosition = _startingPosition - _yTranslation;
    }
}
