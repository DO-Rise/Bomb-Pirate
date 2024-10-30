using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    private BossHealth _health;

    private int _maxStage;
    private int _currentStage = 1;
    private float _nextStageThreshold;
    private float _healthPercentage;

    public void Initialize(int _stage, BossHealth bossHealth)
    {
        _maxStage = _stage;
        _health = bossHealth;

        _healthPercentage = (float)_health.MaxHealth() / _maxStage;
        _nextStageThreshold = _health.MaxHealth() - _healthPercentage;
    }

    public void StageCheck()
    {
        if (_currentStage < _maxStage && _health.CurrentHealth() <= _nextStageThreshold)
        {
            _currentStage++;
            _nextStageThreshold -= _healthPercentage;
        }
    }

    public int CurrentStage()
    {
        return _currentStage;
    }
}
