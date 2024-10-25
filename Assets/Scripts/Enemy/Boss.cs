using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject _bossStartTrigger;

    private bool active = false;

    private void Update()
    {

    }

    public void ActiveBoss(bool activeDeactive)
    {
        active = activeDeactive;
        _bossStartTrigger.SetActive(false);
    }
}
