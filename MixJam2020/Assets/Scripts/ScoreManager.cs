using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int _coinCount = 0;
    private int _starCount = 0;

    private float _timer = 0.0f;


    void Start()
    {
        _timer = 0.0f;
        _coinCount = 0;
        _starCount = 0;
    }

    public void AddItem(CatchItem.ItemType itemType)
    {
        if (itemType == CatchItem.ItemType.Star)
            _starCount++;
        else if (itemType == CatchItem.ItemType.Coin)
            _coinCount++;
    }
}
