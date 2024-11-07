using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject _ball;
    [SerializeField] private Transform _ballSpawnPoint;

    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsAnimationFinished("Attack"))
            _anim.Play("Default");
    }

    public void Attack()
    {
        _anim.Play("Attack");

        Instantiate(_ball, _ballSpawnPoint.position, _ballSpawnPoint.rotation);
    }

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
}
