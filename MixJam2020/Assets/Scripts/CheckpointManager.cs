using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public GameObject _playerCam;
    public GameObject _sphere;
    public GameObject _car;
    public Checkpoint[] checkpoints;

    private int _latestCheckpointId = 0;

    private void Awake()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].SetId(i);
        }
    }

    private void Start()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].Changed += SetLatest;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
        {
            Debug.Log("Go back to the latest checkpoint");
            Transform newtransform = checkpoints[_latestCheckpointId].GetSpawnTransform();

            //_sphere.transform.parent = _player.transform;
            _sphere.GetComponent<Rigidbody>().velocity = Vector3.zero;

            _sphere.transform.position = newtransform.position;
            _sphere.transform.rotation = newtransform.rotation;

            _playerCam.transform.position = newtransform.position;
            _playerCam.transform.rotation = newtransform.rotation;

            _car.transform.rotation = newtransform.rotation;


            //_sphere.transform.parent = null;
        }
    }

    private void SetLatest(int i)
    {
        _latestCheckpointId = i;
        Debug.Log("Latest checkpoint is: " + _latestCheckpointId);
    }
}
