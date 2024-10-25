using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICucumber : MonoBehaviour
{
    [Header("Left Cucumber")]
    [SerializeField] private RectTransform _leftCucumber;
    [SerializeField] private RectTransform _startPointL;
    [SerializeField] private RectTransform _finishPointL;

    [Header("Right Cucumber")]
    [SerializeField] private RectTransform _rightCucumber;
    [SerializeField] private RectTransform _startPointR;
    [SerializeField] private RectTransform _finishPointR;

    private float _speed = 1500f;

    private bool _activeL = false;
    private bool _activeR = false;

    private void Start()
    {
        _leftCucumber.position = _startPointL.position;
        _rightCucumber.position = _startPointR.position;
    }

    private void Update()
    {
        if (_activeL)
            _leftCucumber.position = Vector2.MoveTowards(_leftCucumber.position, _finishPointL.position, _speed * Time.deltaTime);
        else
            _leftCucumber.anchoredPosition = Vector2.MoveTowards(_leftCucumber.anchoredPosition, _startPointL.position, _speed * Time.deltaTime);
        
        if (_activeR)
            _rightCucumber.position = Vector2.MoveTowards(_rightCucumber.position, _finishPointR.position, _speed * Time.deltaTime);
        else
            _rightCucumber.anchoredPosition = Vector2.MoveTowards(_rightCucumber.anchoredPosition, _startPointR.position, _speed * Time.deltaTime);
    }

    public void ActiveCucumber()
    {
        int number = Random.Range(0, 7);

        if (number != 6)
        {
            if (number < 3)
                _activeL = true;
            else
                _activeR = true;
        }
        else
        {
            _activeL = true;
            _activeR = true;
        }
    }

    public void DeactiveCucumber()
    {
        _activeL = false;
        _activeR = false;
    }
}
