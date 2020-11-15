using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public int _startAtCheckpoint = -1;
    public GameObject _deathMenu;

    [Space(10)]
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

#if UNITY_EDITOR
        if (_startAtCheckpoint > 0 && _startAtCheckpoint < checkpoints.Length)
            Respawn(_startAtCheckpoint);
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("enter") || Input.GetKeyDown("return"))
        {
            _deathMenu.SetActive(false);
            Respawn();
        }
    }

    public void Respawn()
    {
        Respawn(_latestCheckpointId);
    }

    public void Respawn(int i)
    {
        Transform newtransform = checkpoints[i].GetSpawnTransform();
        Debug.Log("Go back to checkpoint " + i);

        //_sphere.transform.parent = _player.transform;
        _sphere.GetComponent<Rigidbody>().velocity = Vector3.zero;

        _sphere.transform.position = newtransform.position;
        _sphere.transform.rotation = newtransform.rotation;

        _playerCam.transform.position = newtransform.position;
        _playerCam.transform.rotation = newtransform.rotation;

        _car.transform.rotation = newtransform.rotation;


        //_sphere.transform.parent = null;
    }

    private void SetLatest(int i)
    {
        _latestCheckpointId = i;
        Debug.Log("Latest checkpoint is: " + _latestCheckpointId);
    }
}
