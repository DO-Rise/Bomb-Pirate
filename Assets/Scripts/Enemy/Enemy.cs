using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Name")]
    [SerializeField] private string _enemyName;
    [Space(15)]

    [Header("Collider")]
    [SerializeField] private Collider2D _basicCollider;
    [SerializeField] private Collider2D _deadCollider;
    [Space(15)]

    [Header("Sound")]
    [SerializeField] private AudioClip _attackSound;

    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _sprite;
    private AudioSource _audioSource;

    private PlayerController _playerController;

    private Transform _player;
    private List<Transform> _listsBombs = new List<Transform>();
    private Transform _target;

    private float _speed = 5f;
    private float _detectionRangeX = 7f;
    private float _minDetectionRangeY = -1f;
    private float _maxDetectionRangeY = 3f;

    private bool _dead = false;
    private bool _attack = false;
    private string _vectorAttack = "Null";

    private bool _scare = false;
    private float _secScare = 3f;

    private bool _soundPlayed = false;
    private bool _soundDelay = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();

        _playerController = FindObjectOfType<PlayerController>();

        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (!_dead)
        {
            UpdateBombsList();

            Transform nearestBomb = FindNearestBomb();

            if (nearestBomb != null)
            {
                _target = nearestBomb;

                if (GameUI.Instance.SoundCheck())
                    PlaySound(_attackSound);
            }
            else if (IsTargetValid(_player, _detectionRangeX))
            {
                _target = _player;

                if (GameUI.Instance.SoundCheck())
                    PlaySound(_attackSound);
            }
            else
            {
                _target = null;
                _anim.Play("Idle");

                _soundPlayed = false;
            }

            if (_target != null && !_attack)
                MoveTowardsTarget();

            if (_attack)
            {
                if (IsAnimationFinished("PlayerAttack"))
                {
                    if (IsTargetValid(_player, 2f))
                        _playerController.Damage(_vectorAttack, _enemyName);

                    _anim.Play("Idle");
                    Invoke("AttackDeactive", 1f);
                }
                if (IsAnimationFinished("BombAttack"))
                {
                    if (_enemyName == "Cucumber")
                        Bomb.Instance.BombOff();
                    if (_enemyName == "BaldPirate")
                        Bomb.Instance.BombHit(_vectorAttack, 20f, 20f);
                    if (_enemyName == "BigGuy")
                        Bomb.Instance.BombHit(_vectorAttack, 30f, 1f);
                    if (_enemyName == "Whale")
                        Bomb.Instance.BombEat();

                    _anim.Play("Idle");
                    Invoke("AttackDeactive", 1f);
                }
            }

            if (_scare)
            {
                if (IsAnimationFinished("ScareRun"))
                {
                    _secScare -= Time.deltaTime;

                    if (_secScare <= 0f)
                    {
                        _secScare = 3f;
                        _scare = false;
                    }
                }
            }
        }

        if (IsAnimationFinished("DeadHit"))
            _anim.Play("Dead");
    }

    private void UpdateBombsList()
    {
        _listsBombs.Clear();
        GameObject[] bombObjects = GameObject.FindGameObjectsWithTag("Bomb");

        foreach (GameObject bomb in bombObjects)
        {
            Animator bombAnimation = bomb.GetComponent<Animator>();

            if (bombAnimation != null)
            {
                AnimatorStateInfo currentState = bombAnimation.GetCurrentAnimatorStateInfo(0);

                if (!currentState.IsName("Off") && IsTargetValid(bomb.transform, _detectionRangeX))
                    _listsBombs.Add(bomb.transform);
            }
        }
    }

    private Transform FindNearestBomb()
    {
        Transform nearestBomb = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform bomb in _listsBombs)
        {
            float distance = Mathf.Abs(transform.position.x - bomb.position.x);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestBomb = bomb;
            }
        }

        return nearestBomb;
    }

    private bool IsTargetValid(Transform target, float axisX)
    {
        float distanceX = Mathf.Abs(transform.position.x - target.position.x);
        float distanceY = target.position.y - transform.position.y;

        return distanceX <= axisX && distanceY >=
            _minDetectionRangeY && distanceY <= _maxDetectionRangeY;
    }

    private void MoveTowardsTarget()
    {
        if (transform.position.x > _target.position.x)
            _sprite.flipX = false;
        if (transform.position.x < _target.position.x)
            _sprite.flipX = true;
        
        Vector3 direction = new Vector3(Mathf.Sign(_target.position.x - transform.position.x), 0, 0);

        if (_enemyName == "Captain" && _target != _player)
        {
            if (_sprite.flipX)
                _sprite.flipX = false;
            else
                _sprite.flipX = true;

            transform.position -= direction * _speed * Time.deltaTime;
            _anim.Play("ScareRun");

            _scare = true;
        }
        else
        {
            if (!_scare)
            {
                transform.position += direction * _speed * Time.deltaTime;
                _anim.Play("Run");
            }
        }
    }

    private void AttackDeactive()
    {
        _attack = false;
    }

    private bool IsAnimationFinished(string nameAnim)
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(nameAnim))
        {
            if (stateInfo.normalizedTime >= 1.0f)
                return true;
        }
        return false;
    }

    private void PlaySound(AudioClip audio)
    {
        if (!_soundPlayed && !_soundDelay)
        {
            _audioSource.PlayOneShot(audio);
            _soundPlayed = true;
            _soundDelay = true;

            Invoke("DelaySound", 2f);
        }
    }

    private void DelaySound()
    {
        _soundDelay = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_dead)
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
                _vectorAttack = "Right";
            else
                _vectorAttack = "Left";

            if (collision.gameObject.CompareTag("Player"))
            {
                _anim.Play("PlayerAttack");
                _attack = true;
            }
            if (collision.gameObject.CompareTag("Bomb"))
            {
                _anim.Play("BombAttack");
                _attack = true;
            }
        }
    }

    public void Death(string vectorDamage)
    {
        _dead = true;

        _basicCollider.enabled = false;
        _deadCollider.enabled = true;

        if (vectorDamage == "Left")
        {
            _rb.velocity = Vector2.zero;
            Vector2 bounce = new Vector2(-10f, 5f);

            _rb.AddForce(bounce, ForceMode2D.Impulse);
        }
        else if (vectorDamage == "Left")
        {
            _rb.velocity = Vector2.zero;
            Vector2 bounce = new Vector2(10f, 5f);

            _rb.AddForce(bounce, ForceMode2D.Impulse);
        }

        _anim.Play("DeadHit");
    }
}
