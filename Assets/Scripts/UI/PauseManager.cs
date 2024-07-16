using AudioScripts.AudioSettings;
using EventScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using AudioType = AudioScripts.AudioSettings.AudioType;

public class PauseManager : MonoBehaviour
{
    private static PauseManager _instance;
    public static PauseManager Instance { get { return _instance; } }

    private GameObject _pauseScreen;
    private bool _isPauseScreenOpen;

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
    #endregion
}


