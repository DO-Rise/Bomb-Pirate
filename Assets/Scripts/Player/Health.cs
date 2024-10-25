using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Health Instance;

    [SerializeField] private GameObject[] _hearts;

    private int _currentHearts = 3;
    private bool _deactive = true;

    private void Start()
    {
        Instance = this;
    }

    public void DeactiveHeart()
    {
        if (_currentHearts <= 1)
            PlayerController.Instance.Death();

        if (_currentHearts > 0 && _deactive)
        {
            _hearts[_currentHearts - 1].SetActive(false);
            _currentHearts--;

            _deactive = false;
            Invoke("Pause", 1f);
        }
    }

    public void ActiveHeart()
    {
        if (_currentHearts < 3)
        {
            _currentHearts++;
            _hearts[_currentHearts - 1].SetActive(true);
        }
    }

    private void Pause()
    {
        _deactive = true;
    }
}
