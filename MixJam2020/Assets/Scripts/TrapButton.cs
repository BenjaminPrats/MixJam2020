using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapButton : MonoBehaviour
{
    public MovingTraps[] traps;
    public float _duration = 5.0f;
    public float _yTrigger = 0.2f;

    bool _timerActive = false;
    float _timeRemaining;

    // Update is called once per frame
    void Update()
    {
        if (_timerActive)
        {
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining < 0.0f)
            {
                for (int i = 0; i < traps.Length; i++)
                {
                    traps[i].GoUp();
                }
                transform.position = transform.position + _yTrigger * Vector3.up;
                _timerActive = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CarController carController = other.gameObject.transform.parent.gameObject.GetComponent<CarController>();
        if (carController)
        {
            _timerActive = false;

            for (int i = 0; i < traps.Length; i++)
            {
                traps[i].GoDown();
            }

            transform.position = transform.position - _yTrigger * Vector3.up;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CarController carController = other.gameObject.transform.parent.gameObject.GetComponent<CarController>();
        if (carController)
        {
            _timerActive = true;
            _timeRemaining = _duration;
        }
    }
}
