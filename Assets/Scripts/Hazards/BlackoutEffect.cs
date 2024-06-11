using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class BlackoutEffect : MonoBehaviour
{

    [SerializeField] private GameObject _blackOut;
    [SerializeField] private float _hazardDuration = 5f;
    void Update()
    {
        if (Input.GetKeyDown("k"))
        {
            StartCoroutine("blackout");
        }
    }
    
    IEnumerator blackout()
    {
        _blackOut.SetActive(true);
        Debug.Log("Screen goes bye :<");
        yield return new WaitForSeconds(_hazardDuration);
        Debug.Log("Screen goes hi!");
        _blackOut.SetActive(false);
    }
}
