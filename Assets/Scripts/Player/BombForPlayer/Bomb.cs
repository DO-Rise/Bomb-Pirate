using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public static Bomb Instance;

    public static GameObject BombTake;

    [SerializeField] private string _startAnimation;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _bossLayer;
    [SerializeField] private GameObject _takeUI;

    private BombManager _bombManager;

    private Rigidbody2D _rb;
    private Animator _anim;

    private float _distanceDamage = 1.5f;

    private bool _timer = false;
    private float _secMaxValue = 3f;
    private float _sec;

    private string _vectorDamage = "Null";

    private int _indexLayerBomb;
    private int _indexLayerEnemy;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _anim.Play(_startAnimation);
    }

    private void Start()
    {
        Instance = this;

        _rb = GetComponent<Rigidbody2D>();
        _bombManager = FindObjectOfType<BombManager>();

        _sec = _secMaxValue;

        _indexLayerBomb = LayerMask.NameToLayer("Bomb");
        _indexLayerEnemy = LayerMask.NameToLayer("Enemy");
    }

    private void Update()
    {
        if (IsAnimationPlaying("On"))
        {
            _timer = true;
            Physics2D.IgnoreLayerCollision(_indexLayerBomb, _indexLayerEnemy, false);
        }
        else
            _timer = false;

        if (_timer)
        {
            _sec -= Time.deltaTime;

            if (_sec < 0)
            {
                _sec = _secMaxValue;
                _anim.Play("Boom");

                _timer = false;
            }
        }

        if (IsAnimationPlaying("Boom"))
            FindAndDestroyObjects();

        if (IsAnimationFinished("Boom"))
            _bombManager.DeactiveBomb(gameObject);
    }

    private void FindAndDestroyObjects()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, _distanceDamage, _enemyLayer);
        Collider2D[] playerInRange = Physics2D.OverlapCircleAll(transform.position, _distanceDamage, _playerLayer);

        Collider2D bossInRange = Physics2D.OverlapCircle(transform.position, _distanceDamage, _bossLayer);

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

        if (bossInRange != null)
            bossInRange.GetComponent<BossHealth>().Damage();
    }

    public void BombHit(string vector, float x, float y)
    {
        if (vector == "Left")
        {
            _rb.velocity = Vector2.zero;

            Vector2 bounce = new Vector2(-x, y);
            _rb.AddForce(bounce, ForceMode2D.Impulse);
        }
        else if (vector == "Right")
        {
            _rb.velocity = Vector2.zero;

            Vector2 bounce = new Vector2(x, y);
            _rb.AddForce(bounce, ForceMode2D.Impulse);
        }
    }

    public void BombOff()
    {
        if (!IsAnimationPlaying("Boom"))
        {
            _timer = false;

            _anim.Play("Off");
            _sec = _secMaxValue;

            Physics2D.IgnoreLayerCollision(_indexLayerBomb, _indexLayerEnemy, true);
        }
    }

    public void BombEat()
    {
        _timer = false;

        _sec = _secMaxValue;
        gameObject.SetActive(false);
    }

    public void AnimationPlay(string name)
    {
        _anim.Play(name);
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
        if (DeviceControl.Instance.CurrentDevice() == "PC")
        {
            if (collider.CompareTag("Player") && IsAnimationPlaying("Off"))
                _takeUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (DeviceControl.Instance.CurrentDevice() == "PC")
        {
            if (collider.CompareTag("Player"))
                _takeUI.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _distanceDamage);
    }
}
