using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private string _controller = "PC";

    private Rigidbody2D _rb;
    private Animator _anim;
    private Health _health;

    private bool _movement = true;
    private char _isMove = 'N';
    private float _moveSpeed = 8f;

    private float _jumpForce = 14f;
    private bool _isJumping = true;

    private bool _isDead = false;

    private void Start()
    {
        Instance = this;

        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _health = GetComponent<Health>();

        _anim.Play("Idle");
    }

    private void Update()
    {
        if (_movement)
        {
            if (_controller == "Mobile")
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
                        gameObject.transform.localScale = new Vector3(-3f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                        _rb.velocity = new Vector2(-_moveSpeed, _rb.velocity.y);
                    }
                    else if (_isMove == 'R')
                    {
                        gameObject.transform.localScale = new Vector3(3f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                        _rb.velocity = new Vector2(_moveSpeed, _rb.velocity.y);
                    }
                }
            }
            else if (_controller == "PC")
            {
                float horizontal = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");

                _rb.velocity = new Vector2(horizontal * _moveSpeed, _rb.velocity.y);

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
            gameObject.transform.localScale = new Vector3(3f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

            _rb.velocity = Vector2.zero;
            Vector2 bounce = new Vector2(-10f, 5f);

            _rb.AddForce(bounce, ForceMode2D.Impulse);

            _anim.Play("Hit");
        }
        else if (vector == "Right")
        {
            _movement = false;
            gameObject.transform.localScale = new Vector3(-3f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

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
        if (collider.gameObject.CompareTag("Bomb"))
        {
            GameUI.Instance.BombSelection(collider.gameObject);
            GameUI.Instance.ButtonActive(true, "TakeBomb");
        }

        if (collider.gameObject.CompareTag("TriggerBoss"))
        {
            _movement = false;

            Boss.Instance.ActiveBoss(true);
            GameUI.Instance.AnimationBossImage("Start");
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bomb"))
            GameUI.Instance.ButtonActive(false, "Use");
    }
}