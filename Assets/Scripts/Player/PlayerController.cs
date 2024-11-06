using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _anim;
    private SpriteRenderer _sprite;
    
    private Health _health;

    private string _device;

    private bool _movement = true;
    private char _isMove = 'N';
    private float _moveSpeed = 8f;

    private float _jumpForce = 14f;
    private bool _isJumping = true;

    private bool _isDead = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _health = GetComponent<Health>();

        _device = DeviceControl.Instance.CurrentDevice();

        _anim.Play("Idle");
    }

    private void Update()
    {
        if (_movement)
        {
            if (_device == "Mobile")
            {
                if (_isMove == 'N')
                {
                    if (_isJumping)
                        _anim.Play("Idle");

                    _rb.velocity = new Vector2(0f, _rb.velocity.y);
                }
                else
                {
                    if (_isJumping)
                        _anim.Play("Run");

                    if (_isMove == 'L')
                    {
                        _sprite.flipX = false;
                        _rb.velocity = new Vector2(-_moveSpeed, _rb.velocity.y);
                    }
                    else if (_isMove == 'R')
                    {
                        _sprite.flipX = true;
                        _rb.velocity = new Vector2(_moveSpeed, _rb.velocity.y);
                    }
                }
            }
            else if (_device == "PC")
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                _rb.velocity = new Vector2(horizontal * _moveSpeed, _rb.velocity.y);

                if (horizontal != 0f && _isJumping)
                    _anim.Play("Run");

                if (horizontal == 0f && _isJumping)
                    _anim.Play("Idle");

                if (horizontal > 0f)
                    _sprite.flipX = true;
                else if (horizontal < 0f)
                    _sprite.flipX = false;

                if (Input.GetKey(KeyCode.Space))
                    Jump();
            }
        }

        if (IsAnimationFinished("Hit"))
            _movement = true;

        if (IsAnimationFinished("DeadHit"))
            _anim.Play("Dead");

        if (GameUI.Instance.IsAnimationFinished("End"))
            _movement = true;
    }

    public bool SpriteFlip()
    {
        return _sprite.flipX;
    }

    // Movement
    public void MoveLeft()  { _isMove = 'L'; }

    public void MoveRight() { _isMove = 'R'; }

    public void StopMovement()  { _isMove = 'N'; }

    public void Jump()
    {
        if (_isJumping)
        {
            _anim.Play("Jump");
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);

            _isJumping = false;
        }
    }

    // Damage
    public void Damage(string vector, string enemyName)
    {
        if (vector == "Left")
        {
            _movement = false;
            _sprite.flipX = false;

            _rb.velocity = Vector2.zero;
            Vector2 bounce = new Vector2(-10f, 5f);

            _rb.AddForce(bounce, ForceMode2D.Impulse);

            _anim.Play("Hit");
        }
        else if (vector == "Right")
        {
            _movement = false;
            _sprite.flipX = true;

            _rb.velocity = Vector2.zero;
            Vector2 bounce = new Vector2(10f, 5f);

            _rb.AddForce(bounce, ForceMode2D.Impulse);

            _anim.Play("Hit");
        }

        _health.DeactiveHeart();

        if (enemyName == "BigGuy")
            _health.DeactiveHeart();
    }

    public bool PlayerCheckDeath()
    {
        if (_isDead)
            return true;
        else
            return false;
    }

    public void Death()
    {
        _isDead = true;
        _anim.Play("DeadHit");

        GameUI.Instance.Lose();
    }

    // anim finished
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

    // Trigger & Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            _isJumping = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Bomb"))
        {
            GameUI.Instance.ButtonActive(true, "TakeBomb");

            if (DeviceControl.Instance.CurrentDevice() == "PC")
                GameUI.Instance.CurrentBomb(collider.gameObject);
        }

        if (collider.CompareTag("TriggerBoss"))
        {
            _movement = false;
            GameUI.Instance.AnimationBossImage("Start");
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Bomb"))
        {
            GameUI.Instance.ButtonActive(false, "Use");

            if (DeviceControl.Instance.CurrentDevice() == "PC")
                GameUI.Instance.CurrentBomb(null);
        }
    }
}
