using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;    
using DG.Tweening;
using EventScripts;
using SaveSystem;
using ScriptableData;
public class LoadScreen : MonoBehaviour
{
    [Header("Loading Screen")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressCount;
    public int sceneInt;

    [Header("Save System Referneces")] 
    [SerializeField] private SaveDataManager saveDataManager;
    
    public void LoadScene(int sceneInt)
    {
        StartCoroutine(LoadSceneAsync(sceneInt));
    }
    
    private IEnumerator LoadSceneAsync(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        if (scene is 2 or 1)
        {
            LocalStorageEvents.OnLoadUpgradeData?.Invoke();
            LocalStorageEvents.OnLoadPlayerStats?.Invoke();
            LocalStorageEvents.OnLoadCurrencyData?.Invoke();
            LocalStorageEvents.OnLoadGameStateData?.Invoke();
            LocalStorageEvents.OnLoadButtonSettingsData?.Invoke();
            LocalStorageEvents.OnLoadAudioSettingsData?.Invoke();
        }

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress /.9f);

            progressBar.value = progress;
            progressCount.text = progress * 100 + "%";

            yield return null;
        }
    }
}
