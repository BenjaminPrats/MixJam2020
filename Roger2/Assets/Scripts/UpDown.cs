using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    public float _speed = 1.0f;
    public float _max = 1.0f;
    public float _min = -1.0f;

    private Vector3 _initialPos = Vector3.zero;
    private float _current = 0.0f;
    private float _sign = 1.0f; // 1 or -1

    public Vector3 direction = Vector3.up;


    private void Start()
    {
        _initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        _current += _sign * Time.deltaTime * _speed;
        transform.position = _initialPos + _current * direction;
        if (_current > _max)
        {
            _current = _max;
            _sign = -1.0f;
        }
        if (_current < _min)
        {
            _current = _min;
            _sign = 1.0f;
        }
    }
}
