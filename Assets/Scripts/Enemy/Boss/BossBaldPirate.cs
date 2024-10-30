using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaldPirate : Boss
{
    [SerializeField] private GameObject _allPlatforms;
    [SerializeField] private GameObject _mainCannon;

    private Transform _currentPosition;

    private float _speed = 8f;
    private float _jumpForce = 25f;
    private float _targetHeight = 20f;

    private bool _jumped = false;
    private bool _movingUp = false;
    private bool _falling = false;

    protected override void FirstStage()
    {
        if (!_jumped)
        {
            _anim.Play("Jump");

            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumped = true;

            Invoke("CannonActive", 0.5f);
        }

        if (_movingUp)
        {
            _anim.Play("Cannon");

            if (transform.position.y >= _targetHeight)
            {
                _movingUp = false;

                _rb.velocity = Vector2.zero;
                _rb.gravityScale = 0;

                _mainCannon.SetActive(false);
            }
            else
            {
                _rb.velocity = new Vector2(0, _speed);
                _mainCannon.transform.position = new Vector2(_mainCannon.transform.position.x, _mainCannon.transform.position.y + _speed * Time.deltaTime);
            }
        }

        if (_falling)
            _rb.gravityScale = 1;
    }

    private void CannonActive()
    {
        _mainCannon.SetActive(true);
        _mainCannon.GetComponent<Animator>().Play("Attack");

        Invoke("MoveUpActive", 1f);
    }

    private void MoveUpActive()
    {
        _movingUp = true;
    }

    protected override void SecondStage()
    {

    }

    protected override void ThirdStage()
    {

    }
}
