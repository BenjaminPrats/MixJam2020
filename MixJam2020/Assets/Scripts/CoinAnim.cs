using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnim : MonoBehaviour
{
    public float _rotationStep = 0.5f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0.0f, _rotationStep, 0.0f));
    }
}
