using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject _bossStartTrigger;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxStage;
    [SerializeField] private GameObject _winObject;

    protected Rigidbody2D _rb;
    protected Animator _anim;
    protected SpriteRenderer _sprite;

    protected BossStage _stage;
    protected BossHealth _health;

    protected GameObject _player;

    protected bool _takingDamage = false;
    private bool _death = false;

    private bool _active = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();

        _health = GetComponent<BossHealth>();
        _health.Initialize(_maxHealth);

        _stage = GetComponent<BossStage>();
        _stage.Initialize(_maxStage, _health);

        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (_active)
        {
            switch (_stage.CurrentStage())
            {
                case 1:
                    FirstStage();
                    break;

                case 2:
                    SecondStage();
                    break;

                case 3:
                    ThirdStage();
                    break;
            }
        }

        if (IsAnimationFinished("DeadHit"))
        {
            _anim.Play("Dead");
            _winObject.SetActive(true);
        }
    }

    protected abstract void FirstStage();
    protected abstract void SecondStage();
    protected abstract void ThirdStage();

    public void ActiveBoss()
    {
        _active = true;
        _bossStartTrigger.SetActive(false);

        BossHealth.ActiveBoss = true;
    }

    public void Hit()
    {
        if (!_death)
        {
            if (_health.CurrentHealth() > 0)
            {
                if (_takingDamage)
                {
                    _anim.Play("Hit");
                    _stage.StageCheck();
                }
            }
            else
            {
                Death();
                _death = true;
            }
        }
    }

    public bool ActiveDamage()
    {
        return _takingDamage;
    }

    /*private bool IsAnimationPlaying(string animName)
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animName))
            return true;

        return false;
    }*/

    protected bool IsAnimationFinished(string nameAnim)
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(nameAnim))
        {
            if (stateInfo.normalizedTime >= 1.0f)
                return true;
        }
        return false;
    }

    protected void Death()
    {
        _takingDamage = false;
        _anim.Play("DeadHit");

        _active = false;
    }
}
