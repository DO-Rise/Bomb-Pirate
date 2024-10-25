using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [Header("Start")]
    [SerializeField] private GameObject startDoor;
    [Space(15)]

    [Header("Levels")]
    [SerializeField] private GameObject[] startLevel;
    [SerializeField] private GameObject[] centerLevel;
    [SerializeField] private GameObject[] endLevel;
    [SerializeField] private GameObject[] _bossLevel;
    [SerializeField] private Transform startPoint;
    [SerializeField] private TextMeshPro _currentNumberLevelText;

    [Header("Enemys")]
    [SerializeField] private GameObject[] _enemys;
    
    [Header("Player")]
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _bomb;

    private GameObject _currentObjectPosition;

    private int _currentNumberLevel = 5;
    private int _createNumber = 0;
    private int _createLevel = 0;
    private int _minCreateLevel = 0;
    private int _maxCreateLevel = 2;

    private int _spawnEnemyNumber = 0;

    private bool _buildLevel;

    private void Start()
    {
        Instance = this;

        startDoor.SetActive(true);
        _buildLevel = false;

        _player.transform.position = startPoint.position;

        //PlayerPrefs.DeleteKey("SavedNumberLevel");
        //_currentNumberLevel = PlayerPrefs.GetInt("SavedNumberLevel", 0);
    }

    private void Update()
    {
        _currentNumberLevelText.text = _currentNumberLevel.ToString();

        SettingsLevelNumberAndEnemy();

        if (_createNumber <= (_createLevel + 2) && _buildLevel)
        {
            if (_createNumber == 0)
            {
                CreateFirstLevel();
                _player.transform.position = startPoint.position;

                _createNumber++;
            }
            else if (_createNumber > 0 && _createNumber < (_createLevel + 2))
            {
                if (_currentNumberLevel == 5 || _currentNumberLevel == 14 || _currentNumberLevel == 28
                    || _currentNumberLevel == 45 || _currentNumberLevel == 70)
                {
                    CreateBossLevel(SelectionPoint(_currentObjectPosition));
                    _createNumber = _createLevel + 2;
                }
                else
                {
                    CreateCenterLevel(SelectionPoint(_currentObjectPosition));
                    _createNumber++;
                }
            }
            else if (_createNumber == (_createLevel + 2))
            {
                CreateLastLevel(SelectionPoint(_currentObjectPosition));
                SpawnEnemy();
                SpawnBomb();

                GameUI.Instance.CreateLevelFinish();
                _createNumber++;
            }
        }
    }

    private void SettingsLevelNumberAndEnemy()
    {
        if (_currentNumberLevel < 5)
        {
            if (_currentNumberLevel < 3)
                _createLevel = _currentNumberLevel;
            else
                _maxCreateLevel = 3;

            _spawnEnemyNumber = 1;
        }
        else if (_currentNumberLevel > 5 && _currentNumberLevel < 14)
        {
            _minCreateLevel = 1;
            _maxCreateLevel = 4;

            _spawnEnemyNumber = 2;
        }
        else if (_currentNumberLevel > 14 && _currentNumberLevel < 28)
        {
            _minCreateLevel = 2;
            _maxCreateLevel = 6;

            _spawnEnemyNumber = 3;
        }
        else if (_currentNumberLevel > 28 && _currentNumberLevel < 45)
        {
            _minCreateLevel = 3;
            _maxCreateLevel = 8;

            _spawnEnemyNumber = 4;
        }
        else if (_currentNumberLevel > 45)
        {
            _minCreateLevel = 4;
            _maxCreateLevel = 10;

            _spawnEnemyNumber = 5;
        }
    }

    public void BuildLevel()
    {
        if (_currentNumberLevel > 2)
            _createLevel = Random.Range(_minCreateLevel, _maxCreateLevel);

        startDoor.SetActive(false);
        _buildLevel = true;
    }

    private void CreateFirstLevel()
    {
        int randomNumber = Random.Range(0, startLevel.Length);
        for (int i = 0; i < startLevel.Length; i++)
        {
            if (i == randomNumber)
            {
                GameObject first = Instantiate(startLevel[i], startPoint.position, Quaternion.identity, this.transform);
                _currentObjectPosition = first;
            }
        }
    }

    private void CreateCenterLevel(Transform point)
    {
        int randomNumber = Random.Range(0, centerLevel.Length);
        for (int i = 0; i < centerLevel.Length; i++)
        {
            if (i == randomNumber)
            {
                GameObject center = Instantiate(centerLevel[i], point.position, Quaternion.identity, this.transform);
                _currentObjectPosition = center;
            }
        }
    }

    private void CreateLastLevel(Transform point)
    {
        int randomNumber = Random.Range(0, endLevel.Length);
        for (int i = 0; i < endLevel.Length; i++)
        {
            if (i == randomNumber)
                Instantiate(endLevel[i], point.position, Quaternion.identity, this.transform);
        }
    }

    private void CreateBossLevel(Transform point)
    {
        int number = 0;

        if (_currentNumberLevel == 5)
            number = 0;
        else if (_currentNumberLevel == 14)
            number = 1;
        else if (_currentNumberLevel == 28)
            number = 2;
        else if (_currentNumberLevel == 45)
            number = 3;
        else if (_currentNumberLevel == 70)
            number = 4;

        for (int i = 0; i < endLevel.Length; i++)
        {
            if (i == number)
            {
                GameObject boss = Instantiate(_bossLevel[i], point.position, Quaternion.identity, this.transform);
                _currentObjectPosition = boss;
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPointEnemy");

        foreach (GameObject point in spawnPoints)
        {
            int number = Random.Range(0, _spawnEnemyNumber);
            int numberSpawn = Random.Range(1, 6);

            if (numberSpawn < 5)
                Instantiate(_enemys[number], point.transform.position, Quaternion.identity);
        }
    }
    
    private void SpawnBomb()
    {
        GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPointBomb");

        foreach (GameObject point in spawnPoints)
        {
            int number = Random.Range(1, 6);

            if (number < 4)
                Instantiate(_bomb, point.transform.position, Quaternion.identity);
        }
    }

    private Transform SelectionPoint(GameObject obj)
    {
        Transform point = obj.transform.Find("Point");
        return point;
    }

    public void ResetLevel()
    {
        _currentNumberLevel++;
        //PlayerPrefs.SetInt("SavedNumberLevel", _currentNumberLevel);

        SceneManager.LoadScene(1);
    }
}
