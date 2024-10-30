using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public static bool ActiveBoss = false;

    private Boss _boss;

    private int _maxHP;
    private int _currentHP;

    private bool _dealDamage = true;

    private void Start()
    {
        _boss = FindObjectOfType<Boss>();
    }

    public void Initialize(int _maxHealth)
    {
        _maxHP = _maxHealth;
        _currentHP = _maxHP;
    }

    public float CurrentHealth()
    {
        return _currentHP;
    }

    public float MaxHealth()
    {
        return _maxHP;
    }

    public void Damage()
    {
        if (_dealDamage)
        {
            _currentHP -= 10;
            _boss.Hit();

            _dealDamage = false;
            StartCoroutine(DealDamageCooldown(10f));
        }
    }

    private IEnumerator DealDamageCooldown(float delay)
    {
        _dealDamage = false;
        yield return new WaitForSeconds(delay);
        _dealDamage = true;
    }
}
