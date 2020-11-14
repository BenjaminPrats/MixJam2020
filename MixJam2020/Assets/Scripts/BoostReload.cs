using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostReload : MonoBehaviour
{
    //Rigidbody _rb;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    _rb = GetComponent<Rigidbody>();
    //}

    float _reloadAmount = 0.5f;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        CarController carController = other.gameObject.transform.parent.gameObject.GetComponent<CarController>();
        if (carController)
        {
            carController.ReloadBoost(_reloadAmount);
            gameObject.SetActive(false);
        }
    }
}
