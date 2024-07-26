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
    public static PauseManager Instance => _instance;

    [SerializeField] private GameObject _pauseScreenPanel;
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
        if (_instance == null) _instance = this;
        else Destroy(this);

        GameEvents.ON_PAUSE += TogglePause;
    }

    private void OnDestroy()
    {
        GameEvents.ON_PAUSE -= TogglePause;
    }

    private void Start()
    {
        _isPauseScreenOpen = false;
        _pauseScreenPanel.SetActive(_isPauseScreenOpen);
    }

    #region Pause Screen
    private void TogglePause()
    {
        _isPauseScreenOpen = !_isPauseScreenOpen;
        _pauseScreenPanel.SetActive(_isPauseScreenOpen);

        if (_isPauseScreenOpen)
        {
            Time.timeScale = 0;
            AudioEvents.ON_STOP_SFX?.Invoke();
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    #endregion

    #region buttons

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

    public void OpenMenuButton()
    {
        GameEvents.ON_PAUSE?.Invoke();
    }
}


