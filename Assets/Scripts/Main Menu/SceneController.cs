using System;
using DG.Tweening;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using EventScripts;
using ScriptableData;

public class SceneController : MonoBehaviour
{
    private static SceneController _instance;
    public static SceneController Instance => _instance;

    private GameStateSO gameStateSO;
    
    LoadScreen loadScreen;
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

    public async void LoadScene(int sceneInt)
    {
        DOTween.KillAll(); 
        
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(3);
        
        while (!loadOperation.isDone)
        {
            await Task.Yield(); 
        }

        Canvas canvas = GameObject.FindObjectOfType<Canvas>();

        if (canvas != null)
        {
            loadScreen = canvas.GetComponentInChildren<LoadScreen>();

            if (loadScreen != null)
            {

                loadScreen.LoadScene(sceneInt);
            }

            Time.timeScale = 1.0f; 
        }
    }


    public void Exit()
    {
        Application.Quit();
    }
    
    
    
}
