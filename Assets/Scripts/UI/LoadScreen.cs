using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;    
using DG.Tweening;
using ScriptableData;
public class LoadScreen : MonoBehaviour
{
    [Header("Loading Screen")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressCount;
    public int sceneInt;
    
    private GameStateSO gameStateSO;
    // Start is called before the first frame update
    void Start()
    {
        gameStateSO = GameManager.Instance.FetchGameStateData();
    }
    public void LoadScene(int sceneInt)
    {
        StartCoroutine(LoadSceneAsync(sceneInt));
    }
    
    private IEnumerator LoadSceneAsync(int scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress /.9f);

            progressBar.value = progress;
            progressCount.text = progress * 100 + "%";

            yield return null;
        }
    }
}
