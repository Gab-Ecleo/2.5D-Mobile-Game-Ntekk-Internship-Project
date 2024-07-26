using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void LoadScene(int sceneInt)
    {
        SceneManager.LoadScene(sceneInt);
    }

    public void ReloadScene()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //uncomment if building
    public void Exit()
    {
        Application.Quit();
    }
}
