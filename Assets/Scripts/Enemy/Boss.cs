using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject _bossStartTrigger;

    private Rigidbody2D _rb;
    private Animator _anim;

    private BossStage _stage;

    private bool active = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _stage = GetComponent<BossStage>();
    }

    private void Update()
    {
        if (IsAnimationFinished("Hit"))
            _anim.Play("Idle");
    }

    public void ActiveBoss()
    {
        active = true;
        _bossStartTrigger.SetActive(false);

        BossHealth.ActiveBoss = true;
    }

    public void StartAnimation(string name)
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
}
