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
    [SerializeField] private GameObject loadScreen;
    [SerializeField] private GameObject controlDisplay;
    [SerializeField] private GameObject LoseScreen;
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

    [Header("Animator")]
    [SerializeField] private Animator _animBossImage;

    private AudioSource _audioSource;

    private GameObject _bombSelection;
    private bool _useDoor = false;

    private bool _sound = true;

    private void Start()
    {
        Instance = this;

        _audioSource = GetComponent<AudioSource>();

        _audioSource.clip = _menuSound;
        if (SoundCheck())
            _audioSource.Play();
    }

    private void Update()
    {
        if (IsAnimationFinished("End"))
        {
            _bossImageScreen.SetActive(false);

        }
    }

    public void StartGame()
    {
        loadScreen.SetActive(true);
        controlDisplay.SetActive(false);

        LevelGenerator.Instance.BuildLevel();
    }

    public void CreateLevelFinish()
    {
        loadScreen.SetActive(false);
        controlDisplay.SetActive(true);

        Timer.Instance.StartTime();

        _audioSource.clip = _gameSound;
        if (SoundCheck())
            _audioSource.Play();
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
                _useButton.interactable = true;

            if (nameButton == "Bomb")
                _bombButton.interactable = true;
        }
        else
        {
            if (nameButton == "Use")
            {
                _useButton.interactable = false;
                _useDoor = false;
            }

            if (nameButton == "Bomb")
                _bombButton.interactable = false;
        }
    }
    
    public void BombSelection(GameObject bomb)
    {
        _bombSelection = bomb;
    }

    public void TakeBombButton()
    {
        if (BombManager.Instance.TakeBomb())
            Bomb.Instance.ActiveBomb(_bombSelection);
    }

    public void BombNumberUI(int number)
    {
        _bombCurentNumberText.text = number.ToString();
    }

    public void UseDoorButton()
    {
        if (_useDoor)
        {
            if (Door.Instance.DoorName() == "Start")
                StartGame();
            else if (Door.Instance.DoorName() == "Finish")
                LevelGenerator.Instance.ResetLevel();
        }
    }

    public void SettingsButton(bool active)
    {
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
        LoseScreen.SetActive(true);
        controlDisplay.SetActive(false);

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