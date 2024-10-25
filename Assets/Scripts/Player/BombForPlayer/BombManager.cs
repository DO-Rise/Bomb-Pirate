using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    public static BombManager Instance;

    [SerializeField] private List<GameObject> bombs;
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private Animator reloadAnim;

    private int _currentNumberBomb;

    private void Start()
    {
        Instance = this;
        _currentNumberBomb = bombs.Count;

        reloadAnim.Play("Wait");
    }

    private void Update()
    {
        GameUI.Instance.BombNumberUI(_currentNumberBomb);

        if (_currentNumberBomb < 1)
            GameUI.Instance.ButtonActive(false, "Bomb");
        else
            GameUI.Instance.ButtonActive(true, "Bomb");

        AnimatorStateInfo stateInfo = reloadAnim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Reload"))
        {
            GameUI.Instance.ButtonActive(false, "Bomb");

            if (stateInfo.normalizedTime >= 1f)
            {
                GameUI.Instance.ButtonActive(true, "Bomb");
                reloadAnim.Play("Wait");
            }
        }
    }

    public void ActiveBomb()
    {
        if (_currentNumberBomb > 0)
        {
            bombs[_currentNumberBomb - 1].transform.position = spawnPosition.position;
            bombs[_currentNumberBomb - 1].SetActive(true);

            Bomb.activeBombAnim = true;

            _currentNumberBomb--;

            reloadAnim.Play("Reload");
        }
    }

    public bool TakeBomb()
    {
        if (_currentNumberBomb < bombs.Count)
        {
            _currentNumberBomb++;
            return true;
        }
        else
            return false;
    }
}
