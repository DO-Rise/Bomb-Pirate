using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaldPirate : Boss
{
    [Header("Bald Pirate")]
    [SerializeField] private GameObject _mainCannon;
    [SerializeField] private GameObject[] _cannons;
    [SerializeField] private GameObject[] _bombs;
    [SerializeField] private Transform _positionSecondStage;
    [SerializeField] private Transform _positionThirdStage;
    [SerializeField] private GameObject _allCannons;
    [SerializeField] private GameObject _platforms;
    [SerializeField] private GameObject _platformsGround;

    private Transform _currentPosition;

    private float _speed = 8f;
    private float _jumpForce = 25f;
    private bool _jumped = false;

    private bool _movingUp = false;
    private bool _movingLeft = false;
    private bool _movingRight = false;
    private bool _stop = false;

    private Vector2 _position;

    private bool _bombActive = false;

    private bool _cannonsActive = false;

    protected override void FirstStage()
    {
        if (!_jumped)
        {
            _anim.Play("Jump");

            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _jumped = true;

            Invoke("MainCannonActive", 0.5f);
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
            _bombActive = false;
            StartCoroutine(AttackFirstStageOne());
        }
    }

    private void MainCannonActive()
    {
        _mainCannon.SetActive(true);
        _mainCannon.GetComponent<Animator>().Play("Attack");

        Invoke("MoveUpActive", 1f);
    }

    private void MoveUpActive()
    {
        _movingUp = true;
    }

    private IEnumerator AttackFirstStageOne()
    {
        /*for (int repeat = 0; repeat < 10; repeat++)
        {
            int bombCount = 0;

            while (bombCount < 3)
            {
                int index = Random.Range(0, _bombs.Length);

                if (!_bombs[index].activeSelf)
                {
                    _bombs[index].SetActive(true);
                    bombCount++;
                }
            }

            yield return new WaitForSeconds(2f);
        }*/

        yield return new WaitForSeconds(2f);

        Falling();
    }

    private void Falling()
    {
        _rb.gravityScale = 5f;
        _takingDamage = true;
    }

    protected override void SecondStage()
    {
        if (IsAnimationFinished("Hit"))
        {
            _anim.Play("Run");
            _position = _rb.position;

            _takingDamage = false;
            _movingLeft = true;
        }

        if (_movingLeft)
        {
            _rb.velocity = new Vector2(-_speed, _rb.velocity.y);

            if (Vector2.Distance(_position, _rb.position) >= 5f)
            {
                _rb.velocity = Vector2.zero;

                _movingLeft = false;
                _movingRight = true;
            }
        }

        if (_movingRight)
        {
            _sprite.flipX = true;
            _rb.velocity = new Vector2(_speed, _rb.velocity.y);

            if (_rb.position.x >= _positionSecondStage.position.x)
            {
                _rb.velocity = Vector2.zero;

                _anim.Play("Idle");
                _sprite.flipX = false;

                Invoke("ActiveAttackSecondStage", 2f);

                _movingRight = false;
            }
        }

        if (_cannonsActive)
        {
            _cannonsActive = false;
            StartCoroutine(SecondStageOne());
        }
    }

    private void ActiveAttackSecondStage()
    {
        _platformsGround.SetActive(true);
        _allCannons.SetActive(true);

        if (_player.transform.position.y < _positionSecondStage.position.y)
        {
            Vector3 newPosition = _player.transform.position;
            newPosition.y = _positionSecondStage.position.y + 1;
            _player.transform.position = newPosition;
        }

        _cannonsActive = true;
    }

    private IEnumerator SecondStageOne()
    {
        /*for (int i = 0; i < 7; i++)
        {
            _cannons[i].GetComponent<Cannon>().Attack();
            yield return new WaitForSeconds(2f);
        }

        for (int i = 13; i >= 7; i--)
        {
            _cannons[i].GetComponent<Cannon>().Attack();
            yield return new WaitForSeconds(2f);
        }*/
            yield return new WaitForSeconds(2f);

        StartCoroutine(SecondStageTwo());
    }

    private IEnumerator SecondStageTwo()
    {
        /*for (int repeat = 0; repeat < 4; repeat++)
        {
            for (int i = 0; i < _cannons.Length; i += 3)
            {
                _cannons[i].GetComponent<Cannon>().Attack();
            }

            yield return new WaitForSeconds(2f);

            for (int i = 1; i < _cannons.Length; i += 3)
            {
                _cannons[i].GetComponent<Cannon>().Attack();
            }

            yield return new WaitForSeconds(2f);
        }*/
            yield return new WaitForSeconds(2f);

        StartCoroutine(SecondStageThree());
    }

    private IEnumerator SecondStageThree()
    {
        /*for (int repeat = 0; repeat < 3; repeat++)
        {
            for (int i = 0; i < _cannons.Length; i++)
            {
                _cannons[i].GetComponent<Cannon>().Attack();
                yield return new WaitForSeconds(0.7f);
            }
        
            for (int i = 13; i >= 0; i--)
            {
                _cannons[i].GetComponent<Cannon>().Attack();
                yield return new WaitForSeconds(0.7f);
            }

            for (int i = 0; i < _cannons.Length; i += 3)
            {
                _cannons[i].GetComponent<Cannon>().Attack();
            }

            yield return new WaitForSeconds(0.7f);

            for (int i = 1; i < _cannons.Length; i += 3)
            {
                _cannons[i].GetComponent<Cannon>().Attack();
            }

            yield return new WaitForSeconds(0.7f);
        }*/
            yield return new WaitForSeconds(0.7f);

        _allCannons.SetActive(false);
        _platformsGround.SetActive(false);
        _platforms.SetActive(false);

        _anim.Play("Cannon");
        _takingDamage = true;
    }

    protected override void ThirdStage()
    {
        if (IsAnimationFinished("Hit"))
        {
            _takingDamage = false;

            _sprite.flipX = true;

            Vector2 bounce = new Vector2(0f, 45f);
            _rb.AddForce(bounce, ForceMode2D.Impulse);

            _movingRight = true;
        }

        if (_movingRight)
        {
            _anim.Play("Run");
            _rb.velocity = new Vector2(_speed, _rb.velocity.y);

            if (_rb.position.x >= _positionThirdStage.position.x)
            {
                _rb.velocity = Vector2.zero;

                _anim.Play("Idle");
                _sprite.flipX = false;

                _movingRight = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ceiling"))
            _stop = true;
    }
}
