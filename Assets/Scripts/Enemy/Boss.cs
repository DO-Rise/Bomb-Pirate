using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public static Boss Instance;

    [SerializeField] private GameObject _bossStartTrigger;

    private bool active = false;

    private void Start()
    {
        Instance = this;
    }

    private void Update()
    {

    }

    public void ActiveBoss(bool activeDeactive)
    {
        active = activeDeactive;
        _bossStartTrigger.SetActive(false);
    }
}
