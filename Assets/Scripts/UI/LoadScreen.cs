using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;    

public class LoadScreen : MonoBehaviour
{
    [Header("Loading Screen")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private TMP_Text progressCount;  
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadSceneAsync(2));
    }
    
    IEnumerator LoadSceneAsync(int scene)
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
