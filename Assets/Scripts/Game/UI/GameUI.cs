using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [Header("Screen")]
    [SerializeField] private GameObject _loadScreen;
    [SerializeField] private GameObject _controlDisplay;
    [SerializeField] private GameObject _controlDisplayButton;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private GameObject _settingsScreen;
    [SerializeField] private GameObject _tutorialScreen;
    [SerializeField] private GameObject _bossImageScreen;
    [Space(15)]

    [Header("Button")]
    [SerializeField] private Button _useButton;
    [SerializeField] private Button _bombButton;
    [SerializeField] private Button _soundOnButton;
    [SerializeField] private Button _soundOffButton;
    [Space(15)]

    [Header("Sound")]
    [SerializeField] private AudioClip _menuSound;
    [SerializeField] private AudioClip _gameSound;
    [SerializeField] private AudioClip _deathSound;
    [Space(15)]

    [Header("Text")]
    [SerializeField] private TMP_Text _bombCurentNumberText;
    [Space(15)]

    [Header("Other")]
    [SerializeField] private Animator _animBossImage;
    [SerializeField] private GameObject _bossHealthObject;
    [SerializeField] private Slider _bossHealthSlider;
    [SerializeField] private LevelGenerator _levelGenerator;
    [SerializeField] private BombManager _bombManager;

    private PlayerController _player;

    private Boss _boss;
    private BossHealth _bossHealth;

    private Timer _timer;
    private AudioSource _audioSource;

    private GameObject _currentTakeBomb;
    private bool _takeBomb = false;
    private bool _plantBomb = true;
    private bool _useDoor = false;

    private bool _sound = true;

    private string _device;
    private int _numberClickEscape = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1f;

        _timer = GetComponentInChildren<Timer>();
        _audioSource = GetComponent<AudioSource>();

        _device = DeviceControl.Instance.CurrentDevice();

        _player = FindObjectOfType<PlayerController>();

        _audioSource.clip = _menuSound;
        if (SoundCheck())
            _audioSource.Play();
    }

    private void Update()
    {
        if (_device == "PC")
        {
            _controlDisplayButton.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_numberClickEscape == 0)
                {
                    SettingsButton(true);
                    _numberClickEscape++;
                }
                else
                {
                    SettingsButton(false);
                    _numberClickEscape--;
                }
            }

            if (Input.GetKey(KeyCode.E))
                UseButton();

            if (Input.GetKeyDown(KeyCode.O) && _plantBomb)
                _bombManager.ActiveBomb();
        }
        else
            _controlDisplayButton.SetActive(true);

        // Boss
        if (IsAnimationFinished("End"))
        {
            _boss.ActiveBoss();

            _player.MovementActive();

            _bossImageScreen.SetActive(false);
            _bossHealthObject.SetActive(true);

            _bossHealthSlider.maxValue = _bossHealth.CurrentHealth();
        }

        if (_bossHealthObject.activeSelf)
            _bossHealthSlider.value = _bossHealth.CurrentHealth();
    }

    public void StartGame()
    {
        _loadScreen.SetActive(true);
        _controlDisplay.SetActive(false);

        _levelGenerator.BuildLevel();
    }

    public void CreateLevelFinish()
    {
        _loadScreen.SetActive(false);
        _controlDisplay.SetActive(true);

        _timer.StartTime();

        _audioSource.clip = _gameSound;
        if (SoundCheck())
            _audioSource.Play();

        _boss = FindObjectOfType<Boss>();
        _bossHealth = FindObjectOfType<BossHealth>();
    }

    public void ButtonActive(bool active, string nameButton)
    {
        if (active)
        {
            if (nameButton == "Door")
            {
                _useButton.interactable = true;
                _useDoor = true;
            }

            if (nameButton == "TakeBomb")
            {
                _useButton.interactable = true;
                _takeBomb = true;
            }

            if (nameButton == "Bomb")
            {
                _bombButton.interactable = true;
                _plantBomb = true;
            }
        }
        else
        {
            if (nameButton == "Use")
            {
                _useButton.interactable = false;
                _useDoor = false;
                _takeBomb = false;
            }

            if (nameButton == "Bomb")
            {
                _bombButton.interactable = false;
                _plantBomb = false;
            }
        }
    }

    public void BombNumberUI(int number)
    {
        _bombCurentNumberText.text = number.ToString();
    }

    public void CurrentBomb(GameObject bomb)
    {
        _currentTakeBomb = bomb;
    }

    public void UseButton()
    {
        if (_useDoor)
        {
            if (Door.Instance.DoorName() == "Start")
                StartGame();
            else if (Door.Instance.DoorName() == "Finish")
                _levelGenerator.ResetLevel();
        }

        if (_takeBomb && _bombManager.TakeBomb())
        {
            _bombManager.DeactiveBomb(_currentTakeBomb);
        }
    }

    public void SettingsButton(bool active)
    {
        Time.timeScale = active ? 0f : 1f;

        _settingsScreen.SetActive(active);
    }
    
    public void TutorialButton(bool active)
    {
        _tutorialScreen.SetActive(active);
    }

    public void ButtonSoundOff(bool click)
    {
        if (!click)
        {
            _sound = true;
            _audioSource.Play();

            _soundOnButton.interactable = false;
            _soundOffButton.interactable = true;
        }
        else
        {
            _sound = false;
            _audioSource.Stop();

            _soundOffButton.interactable = false;
            _soundOnButton.interactable = true;
        }
    }

    public bool SoundCheck()
    {
        return _sound;
    }

    public void AnimationBossImage(string name)
    {
        _bossImageScreen.SetActive(true);
        _animBossImage.Play(name);

        Invoke("EndAnimationBossImage", 2.5f);
    }

    private void EndAnimationBossImage()
    {
        _animBossImage.Play("End");
    }

    public bool IsAnimationFinished(string nameAnim)
    {
        if (_animBossImage != null && _animBossImage.gameObject.activeInHierarchy)
        {
            AnimatorStateInfo stateInfo = _animBossImage.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName(nameAnim))
            {
                if (stateInfo.normalizedTime >= 1.0f)
                    return true;
            }
        }
        return false;
    }

    public void Lose()
    {
        _loseScreen.SetActive(true);
        _controlDisplay.SetActive(false);

        _audioSource.clip = _deathSound;
        if (SoundCheck())
            _audioSource.Play();
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
