using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReappiringItem : MonoBehaviour
{
    // Start is called before the first frame update
    public CatchItem _item;
    public float _delayReapparition = 4.0f;

    private GameObject _object;


    private float _timer = 0.0f;

    void Start()
    {
        if (_item == null || _item.itemType == CatchItem.ItemType.None)
            Debug.LogError("Wrong script attached!");

        _object = _item.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_object.activeSelf)
        {
            _timer += Time.deltaTime;
            if (_timer > _delayReapparition)
                Reappiring();
        }
    }
    
    private void Reappiring()
    {
        _object.SetActive(true);
        _timer = 0.0f;
    }
}
