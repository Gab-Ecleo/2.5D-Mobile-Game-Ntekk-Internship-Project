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

    private PlayerStatsSO initialPlayerStat;
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        initialPlayerStat = GameManager.Instance.FetchInitialPlayerStat();
    }

    public void LoadScene(int sceneInt, bool isDefaultHome)
    {
        DOTween.KillAll();

        initialPlayerStat.stats.isDefaultHomeButton = isDefaultHome;
        LocalStorageEvents.OnSavePlayerStats?.Invoke();
        SceneManager.LoadScene(sceneInt);
        Time.timeScale = 1.0f;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
