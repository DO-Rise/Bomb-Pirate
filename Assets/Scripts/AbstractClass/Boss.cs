using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private GameObject _bossStartTrigger;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxStage;

    protected Rigidbody2D _rb;
    protected Animator _anim;
    protected SpriteRenderer _sprite;

    protected BossStage _stage;
    protected BossHealth _health;

    private bool active = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();

        _health = GetComponent<BossHealth>();
        _health.Initialize(_maxHealth);

        _stage = GetComponent<BossStage>();
        _stage.Initialize(_maxStage, _health);
    }

    private void Update()
    {
        if (active)
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
    }

    protected abstract void FirstStage();
    protected abstract void SecondStage();
    protected abstract void ThirdStage();

    public void ActiveBoss()
    {
        active = true;
        _bossStartTrigger.SetActive(false);

        BossHealth.ActiveBoss = true;
    }

    public void Hit()
    {
        _anim.Play("Hit");
        _stage.StageCheck();
    }

    /*private bool IsAnimationPlaying(string animName)
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(animName))
            return true;

        return false;
    }*/

    /*private bool IsAnimationFinished(string nameAnim)
    {
        AnimatorStateInfo stateInfo = _anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(nameAnim))
        {
            if (stateInfo.normalizedTime >= 1.0f)
                return true;
        }
        return false;
    }*/
}
