using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance;
    public static bool ActiveBombAnim = false;

    [SerializeField] private GameObject _takeUI;
    [SerializeField] private Collider2D _basicCollider;
    [SerializeField] private Collider2D _useCollider;
    [SerializeField] private string _startAnimation = "Off";
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _playerLayer;

    private Animator _anim;
    private Rigidbody2D _rb;

    private float _sec;
    private float _secMaxValue = 3f;
    private bool _time = false;

    private float _distanceDamage = 1.5f;
    private string _vectorDamage = "Null";

    private int _indexLayerBomb;
    private int _indexLayerEnemy;
    private int _indexLayerPlayer;

    private string _device;

    private void Start()
    {
        Instance = this;

        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        _device = DeviceControl.Instance.CurrentDevice();

        _indexLayerBomb = LayerMask.NameToLayer("Bomb");
        _indexLayerEnemy = LayerMask.NameToLayer("Enemy");

        _sec = _secMaxValue;

        _anim.Play(_startAnimation);
    }

    private void Update()
    {
        if (IsAnimationPlaying("Onn"))
        {
            _time = true;
            Physics2D.IgnoreLayerCollision(_indexLayerBomb, _indexLayerEnemy, false);
        }
        if (IsAnimationPlaying("Off"))
            _time = false;

        if (_time)
        {
            _sec -= Time.deltaTime;

            if (_sec <= 0)
            {
                _anim.Play("Boom");
                _sec = _secMaxValue;

                _time = false;
            }
        }
        
        if (IsAnimationPlaying("Boom"))
            FindAndDestroyObjects();

        if (IsAnimationFinished("Boom"))
            gameObject.SetActive(false);

        if (IsAnimationPlaying("Off"))
            _useCollider.enabled = true;
        else
            _useCollider.enabled = false;
    }

    public void ActiveBomb(GameObject objBomb)
    {
        _anim.Play("Onn");
        objBomb.SetActive(false);
    }

    private void FindAndDestroyObjects()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, _distanceDamage, _enemyLayer);
        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(transform.position, _distanceDamage, _playerLayer);

        foreach (Collider2D enemy in enemiesInRange)
        {
            if (enemy.transform.position.x < transform.position.x)
                _vectorDamage = "Left";
            else
                _vectorDamage = "Right";

            enemy.GetComponent<Enemy>().Death(_vectorDamage);
        }
        
        foreach (Collider2D player in playerInRange)
        {
            if (player.transform.position.x < transform.position.x)
                _vectorDamage = "Left";
            else
                _vectorDamage = "Right";

            player.GetComponent<PlayerController>().Damage(_vectorDamage, "Null");
        }
    }

    public void BombHit(string vector, float y)// y = 20 || 0
    {
        if (vector == "Left")
        {
            _rb.velocity = Vector2.zero;

            Vector2 bounce = new Vector2(-20f, y);
            _rb.AddForce(bounce, ForceMode2D.Impulse);
        }
        else if (vector == "Right")
        {
            _rb.velocity = Vector2.zero;

            Vector2 bounce = new Vector2(20f, y);
            _rb.AddForce(bounce, ForceMode2D.Impulse);
        }
    }

    public void BombOff()
    {
        if (!IsAnimationPlaying("Boom"))
        {
            _anim.Play("Off");
            _sec = _secMaxValue;

            Physics2D.IgnoreLayerCollision(_indexLayerBomb, _indexLayerEnemy, true);
        }
    }

    public void BombEat()
    {
        _sec = _secMaxValue;
        gameObject.SetActive(false);
    }

    private bool IsAnimationPlaying(string animName)
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animName))
            return true;

        return false;
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

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (_device == "PC" && collider.CompareTag("Player"))
            _takeUI.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (_device == "PC" && collider.CompareTag("Player"))
            _takeUI.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && ActiveBombAnim)
        {
            _anim.Play("Onn");
            ActiveBombAnim = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distanceDamage);
    }
}
