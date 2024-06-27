using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;


public class BlackoutEffect : MonoBehaviour
{
    [SerializeField] private float hazardDuration = 5f;
    [SerializeField] private Animation anim;

    void Update()
    {
        // CHANGE TRIGGER in the Future!!!  Activates hazard when button is pressed
        if (Input.GetKeyDown("k"))
        {
            StartCoroutine(nameof(Blackout));
        }
        
    }
    // Initiate a blackout which makes the screen go dark after [Hazard Duration].
    IEnumerator Blackout()
    {
        anim.Play("FadeIn");
        Debug.Log("Screen goes bye :<");
        yield return new WaitForSeconds(hazardDuration);
        anim.Play("FadeOut");
        Debug.Log("Screen goes hi!");
    }
    
}
