using AudioScripts.AudioSettings;
using EventScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AudioType = AudioScripts.AudioSettings.AudioType;


public class PauseManager : MonoBehaviour
{
    private static PauseManager _instance;
    public static PauseManager Instance { get { return _instance; } }

    private GameObject _pauseScreen;
    private bool _isPauseScreenOpen;

    [Header("BGM Audio")]
    [SerializeField] private Sprite _bgmOn;
    [SerializeField] private Sprite _bgmOff;
    [SerializeField] private Button _bgmButton;

    [Header("SFX Audio")]
    [SerializeField] private Sprite _sfxOn;
    [SerializeField] private Sprite _sfxOff;
    [SerializeField] private Button _sfxButton;

    private bool _isBgmOn;
    private bool _isSfxOn;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if(_pauseScreen == null) { _pauseScreen = this.gameObject;}

    }

    private void Start()
    {
        _isPauseScreenOpen = false;
        _pauseScreen.SetActive(_isPauseScreenOpen);
    }

    #region Pause Screen
    public void OnPause(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        _isPauseScreenOpen = !_isPauseScreenOpen;
        _pauseScreen.SetActive(_isPauseScreenOpen);

        if (_isPauseScreenOpen)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    #endregion

    #region buttons
    public void LevelSelector(int GoToScene)
    {
        SceneManager.LoadScene(GoToScene);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Resume()
    {
        TogglePause();
    }

    public void MuteBGM()
    {
        _isBgmOn = !_isBgmOn;
        _bgmButton.image.sprite = _isBgmOn ? _bgmOn : _bgmOff;
        AudioManager.Instance.AudioMute(!_isBgmOn, AudioType.BGM);
    }

    public void MuteSFX()
    {
        _isSfxOn = !_isSfxOn;
        _sfxButton.image.sprite = _isSfxOn ? _sfxOn : _sfxOff;
        AudioManager.Instance.AudioMute(!_isSfxOn, AudioType.Sfx);
    }
    #endregion
}


