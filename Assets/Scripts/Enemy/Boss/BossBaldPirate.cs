using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaldPirate : Boss
{
    private Transform _currentPosition;

    private float _speed = 8f;

    protected override void FirstStage()
    {
        _currentPosition = GameObject.Find("PositionUp").transform;

        Vector2 direction = ((Vector2)_currentPosition.position - _rb.position).normalized;
        _rb.velocity = direction * _speed;

        if (Vector2.Distance(_rb.position, _currentPosition.position) < 0.5f)
        {
            _rb.velocity = Vector2.zero;

            _anim.Play("Idle");
            _sprite.flipX = false;
        }
        else
        {
            _sprite.flipX = true;
            _anim.Play("Jump");
        }
    }

    protected override void SecondStage()
    {

    }

    protected override void ThirdStage()
    {

    }
}
