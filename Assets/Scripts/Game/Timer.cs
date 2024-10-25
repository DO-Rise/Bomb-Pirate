using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    [SerializeField] private TMP_Text timerText;

    private bool _time = false;
    private bool _uiCucumber = true;

    private int _min = 0;
    private float _sec = 0f;

    private string _forMin = "";
    private string _forSec = "";

    private void Start()
    {
        Instance = this;
        _time = false;
    }

    private void Update()
    {
        if (_time)
        {
            if (_min < 10)
                _forMin = "0";
            else
                _forMin = "";

            if (_sec < 10)
                _forSec = "0";
            else
                _forSec = "";

            timerText.text = _forMin + _min.ToString() + ":" + _forSec + Mathf.FloorToInt(_sec).ToString();

            _sec += Time.deltaTime;

            if (_sec >= 60)
            {
                _sec = 0f;
                _min++;
            }

            if (_min % 2 == 0 && _sec < 17f)
            {
                if (_sec == 0 && _uiCucumber)
                {
                    UICucumber.Instance.ActiveCucumber();
                    _uiCucumber = false;
                }
                if (_sec > 15 && _sec < 17)
                {
                    UICucumber.Instance.DeactiveCucumber();
                    _uiCucumber = true;
                }
            }
        }
    }

    public void StartTime()
    {
        _time = true;
    }
}
