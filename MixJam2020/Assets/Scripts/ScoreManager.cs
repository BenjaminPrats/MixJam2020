using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public GameObject _endingPanel;
    public Image _bgEnding;
    public Text _endingMessageCoin;
    public Text _endingMessageStar;
    public Text _endingMessageComment;
    public Text _endingMessageTime;


    public Text _timerText;
    public Text _coinText;
    public Text _starText;


    private int _coinCount = 0;
    private int _starCount = 0;

    private int _coinCountMax = 0;
    private int _starCountMax = 0;

    private float _endingTime;
    private float _startTime = 0.0f;
    private bool _stopped = false;

    private float _hue = 0.0f;

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
        _timerText.text = GetTimeString();

        if (_stopped && _endingPanel.activeSelf)
        {
            _hue += 0.1f * Time.deltaTime;
            if (_hue > 1.0f)
                _hue = 0.0f;
            Color newColor = Color.HSVToRGB(_hue, 0.8f, 0.8f);
            newColor.a = 0.8f;
            _bgEnding.color = newColor;
        }

    }

    private string GetTimeString()
    {
        float time = _endingTime;
        if (!_stopped)
        {
            time = Time.time - _startTime;
            _endingTime = time;
        }

        // time text
        string minutes = ((int)time / 60).ToString();
        string seconds = (time % 60).ToString("f2");
        return minutes + ":" + seconds;
    }

    public void AddItem(CatchItem.ItemType itemType)
    {
        if (!_stopped)
        {
            if (itemType == CatchItem.ItemType.Star)
                _starCount++;
            else if (itemType == CatchItem.ItemType.Coin)
                _coinCount++;

            UpdateCounters();
        }
    }

    public void Ended()
    {
        _stopped = true;
        _endingMessageCoin.text = "Coins: " + _coinCount + "/" + _coinCountMax;
        _endingMessageStar.text = "Stars: " + _starCount + "/" + _starCountMax;
        _endingMessageComment.text = (_coinCount >= _coinCountMax && _starCount >= _starCountMax) ? "You caught them all!" : "Not bad!";
        _endingMessageTime.text = GetTimeString();
        _endingPanel.SetActive(true);
    }

    public void ExitEnded()
    {
        _endingPanel.SetActive(false);
    }

    private void UpdateCounters()
    {
        _coinText.text = "Coins:" + _coinCount + "/" + _coinCountMax;
        _starText.text = "Stars: " + _starCount + "/" + _starCountMax;
    }
}
