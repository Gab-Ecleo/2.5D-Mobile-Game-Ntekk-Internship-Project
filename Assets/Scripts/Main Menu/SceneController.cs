using System;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventScripts;
using ScriptableData;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    public static SceneController Instance => _instance;

    private GameStateSO gameStateSO;
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        gameStateSO = GameManager.Instance.FetchGameStateData();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0) return;
            if(Input.GetKey(KeyCode.Escape))
                Exit();
    }

    public void LoadScene(int sceneInt, bool isDefaultHome)
    {
        DOTween.KillAll();

        gameStateSO.isDefaultHomeButton = isDefaultHome;
        LocalStorageEvents.OnSaveGameStateData?.Invoke();
        SceneManager.LoadScene(sceneInt);
        Time.timeScale = 1.0f;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
