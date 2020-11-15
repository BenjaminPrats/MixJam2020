using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Material matOn;
    public Material matOff;

    public bool _isActive = false;

    public delegate void OnChange(int i);
    public event OnChange Changed;
    public Transform _spawPosition;

    private int _id = 0;

    public void SetId(int i)
    {
        _id = i;
    }

    private void OnEnable()
    {
        SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        CarController carController = other.gameObject.transform.parent.gameObject.GetComponent<CarController>();
        if (carController)
        {
            SetActive(true);
        }
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            _isActive = true;
            GetComponent<Collider>().enabled = false;
            GetComponent<Renderer>().material = matOn;
        }
        else
        {
            _isActive = false;
            GetComponent<Collider>().enabled = true;
            GetComponent<Renderer>().material = matOff;
        }

        if (Changed != null)
            Changed(_id);
    }

    public Transform GetSpawnTransform()
    {
        return _spawPosition;
    }

    
}
