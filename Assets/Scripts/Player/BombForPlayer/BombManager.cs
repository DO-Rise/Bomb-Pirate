using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _listBombs;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private Animator _reloadAnim;

    private int _currentNumberBomb;

    private void Start()
    {
        _currentNumberBomb = _listBombs.Count;

        _reloadAnim.Play("Wait");
    }

    private void Update()
    {
        GameUI.Instance.BombNumberUI(_currentNumberBomb);

        if (_currentNumberBomb < 1)
            GameUI.Instance.ButtonActive(false, "Bomb");
        else
            GameUI.Instance.ButtonActive(true, "Bomb");

        AnimatorStateInfo stateInfo = _reloadAnim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Reload"))
        {
            GameUI.Instance.ButtonActive(false, "Bomb");

            if (stateInfo.normalizedTime >= 1f)
            {
                GameUI.Instance.ButtonActive(true, "Bomb");
                _reloadAnim.Play("Wait");
            }
        }
    }

    public void ActiveBomb()
    {
        if (_currentNumberBomb > 0)
        {
            _listBombs[_currentNumberBomb - 1].transform.position = _spawnPosition.position;
            _listBombs[_currentNumberBomb - 1].SetActive(true);

            Bomb.ActiveBombAnim = true;

            _currentNumberBomb--;

            _reloadAnim.Play("Reload");
        }
    }

    public bool TakeBomb()
    {
        if (_currentNumberBomb < _listBombs.Count)
        {
            _currentNumberBomb++;
            return true;
        }
        else
            return false;
    }
}
