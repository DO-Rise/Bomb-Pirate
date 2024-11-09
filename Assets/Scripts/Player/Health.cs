using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private GameObject[] _hearts;

    private PlayerController _playerController;

    private int _currentHearts = 3;
    private bool _deactive = true;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void DeactiveHeart()
    {
        if (_currentHearts < 1)
            _playerController.Death();

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
