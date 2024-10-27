using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    [SerializeField] private int _maxStage;

    private BossHealth _health;

    private int _currentStage = 1;
    private float _healthPercentage;

    private void Start()
    {
        _health = GetComponent<BossHealth>();
    }

    private void Update()
    {
        _healthPercentage = _health.CurrentHealth() / _maxStage;
    }

    public int CurrentStage()
    {
        return _currentStage;
    }
}
