using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaldPirate : Boss
{
    [Header("Bald Pirate")]
    [SerializeField] private GameObject _mainCannon;
    [SerializeField] private GameObject[] _bombs;

    private Transform _currentPosition;

    private float _speed = 8f;
    private float _jumpForce = 25f;

    private bool _jumped = false;
    private bool _movingUp = false;
    private bool _stop = false;

    private bool _bombActive = false;
    private float _sec = 2f;

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

            if (_stop)
            {
                _rb.velocity = Vector2.zero;
                _rb.gravityScale = 0;

                _mainCannon.transform.position = new Vector2(_mainCannon.transform.position.x, _mainCannon.transform.position.y);
                _mainCannon.SetActive(false);

                _bombActive = true;
                _movingUp = false;
            }
            else
            {
                _rb.velocity = new Vector2(0, _speed);
                _mainCannon.transform.position = new Vector2(_mainCannon.transform.position.x, _mainCannon.transform.position.y + _speed * Time.deltaTime);
            }
        }

        if (_bombActive)
        {
            _sec -= Time.deltaTime;

            if (_sec < 0)
            {
                int bombCount = 0;

                while (bombCount < 3)
                {
                    int number = Random.Range(0, _bombs.Length);

                    if (!_bombs[number].activeSelf)
                    {
                        _bombs[number].SetActive(true);
                        bombCount++;
                    }
                }

                _sec = 2f;
            }
        }

        if (_falling)
            _rb.gravityScale = 3.5f;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ceiling"))
            _stop = true;
    }
}
