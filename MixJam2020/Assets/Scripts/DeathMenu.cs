using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Time.timeScale = 0.0f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1.0f;
    }
}
