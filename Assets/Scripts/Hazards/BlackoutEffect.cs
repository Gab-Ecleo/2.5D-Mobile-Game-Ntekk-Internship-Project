using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class BlackoutEffect : MonoBehaviour
{
    
    [SerializeField] private Volume _postProcessVolume;
    private ColorAdjustments _colorAdjustments;
    [SerializeField] private float _hazardDuration = 5f;
    void Start()
    {
        _postProcessVolume = GetComponent<Volume>();
        _postProcessVolume.profile.TryGet(out _colorAdjustments);
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
        //_colorAdjustments.colorFilter.
        _colorAdjustments.colorFilter.value = Color.black;
        yield return new WaitForSeconds(_hazardDuration);
        Debug.Log("Screen goes hi!");
        _colorAdjustments.colorFilter.value = Color.white;  
    }
}
