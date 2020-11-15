using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlineHandler : MonoBehaviour
{
    public ScoreManager _scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        CarController carController = other.gameObject.transform.parent.gameObject.GetComponent<CarController>();
        if (carController)
        {
            _scoreManager.Ended();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CarController carController = other.gameObject.transform.parent.gameObject.GetComponent<CarController>();
        if (carController)
        {
            _scoreManager.ExitEnded();
        }
    }
}
