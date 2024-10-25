using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Plot : MonoBehaviour
{
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.Play("StartPlot");
    }

    private void Update()
    {
        if (IsAnimationFinished("StartPlot"))
            SceneManager.LoadScene(1);
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

    public void SkipPlot()
    {
        SceneManager.LoadScene(1);
    }
}
