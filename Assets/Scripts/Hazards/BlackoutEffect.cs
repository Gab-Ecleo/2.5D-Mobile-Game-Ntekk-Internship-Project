using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.PostProcessing;


public class BlackoutEffect : MonoBehaviour
{
    
    [SerializeField] private PostProcessVolume _postProcessVolume;
    private ColorGrading _colorGrading;
    [SerializeField] private float _hazardDuration = 5f;
    void Start()
    {
        _postProcessVolume = GetComponent<PostProcessVolume>();
        _postProcessVolume.profile.TryGetSettings(out _colorGrading);
    }
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            StartCoroutine("blackout");
        }
    }
    
    IEnumerator blackout()
    {
        Debug.Log("Screen goes bye :<");
        _colorGrading.colorFilter.value = Color.black;
        yield return new WaitForSeconds(_hazardDuration);
        Debug.Log("Screen goes hi!");
        _colorGrading.colorFilter.value = Color.white;  
    }
}
