using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text _timerText;
    public Text _coinText;
    public Text _starText;


    private int _coinCount = 0;
    private int _starCount = 0;

    private int _coinCountMax = 0;
    private int _starCountMax = 0;

    private float _startTime = 0.0f; 

    void Start()
    {
        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        GameObject[] stars = GameObject.FindGameObjectsWithTag("Star");
        _coinCountMax = coins.Length;
        _starCountMax = stars.Length;

        _startTime = Time.time;
        _coinCount = 0;
        _starCount = 0;

        UpdateCounters();
    }

    private void Update()
    {
        float time = Time.time - _startTime;

        // time text
        string minutes = ((int)time / 60).ToString();
        string seconds = (time % 60).ToString("f2");
        _timerText.text = minutes + ":" + seconds;
    }

    public void AddItem(CatchItem.ItemType itemType)
    {
        if (itemType == CatchItem.ItemType.Star)
            _starCount++;
        else if (itemType == CatchItem.ItemType.Coin)
            _coinCount++;

        UpdateCounters();
    }

    private void UpdateCounters()
    {
        _coinText.text = _coinCount + "/" + _coinCountMax;
        _starText.text = _starCount + "/" + _starCountMax;
    }
}
