using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public static bool ActiveBoss = false;

    [SerializeField] private int _maxHP;
    
    private Boss _boss;

    private int _currentHP;
    private float _percentageHP;

    private bool _dealDamage = true;

    private void Start()
    {
        _boss = GetComponent<Boss>();

        _currentHP = _maxHP;
    }

    private void Update()
    {
        _currentHP = Mathf.Clamp(_currentHP, 0, _maxHP);
        _percentageHP = (_currentHP / _maxHP) * 100;
    }

    public int SliderHealthBoss()
    {
        return _currentHP;
    }

    public float CurrentHealth()
    {
        return _percentageHP;
    }

    public void Damage()
    {
        if (_dealDamage)
        {
            _currentHP -= 10;
            _boss.StartAnimation("Hit");

            _dealDamage = false;
            Invoke("DealDamageActive", 10f);
        }
    }

    private void DealDamageActive()
    {
        _dealDamage = true;
    }
}
