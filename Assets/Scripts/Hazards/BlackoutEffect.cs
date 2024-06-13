using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class BlackoutEffect : MonoBehaviour
{
    [SerializeField] private float _hazardDuration = 5f;
    [SerializeField] private Animation anim;

    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            StartCoroutine("blackout");
        }
        
    }
    IEnumerator blackout()
    {
        anim.Play("FadeIn");
        Debug.Log("Screen goes bye :<");
        yield return new WaitForSeconds(_hazardDuration);
        anim.Play("FadeOut");
        Debug.Log("Screen goes hi!");
    }
    
}
