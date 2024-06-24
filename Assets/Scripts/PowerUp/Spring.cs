using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private float _powerDuration = 3f;
    [SerializeField] private PlayerStatsSO currentPlayerStats;
    [SerializeField] private float _currentJumpHeight;
    [SerializeField] private float _jumpBoost = 3;
    
    
    private void Start()
    {
        _currentJumpHeight = currentPlayerStats.jumpHeight;
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine("SpringPower");
        
        //Destroy(gameObject);
    }

    IEnumerator SpringPower()
    {
        _currentJumpHeight += _jumpBoost;
        currentPlayerStats.jumpHeight = _currentJumpHeight;
        Debug.Log("Spring On");
        yield return new WaitForSeconds(_powerDuration);
        _currentJumpHeight -= _jumpBoost;
        currentPlayerStats.jumpHeight = _currentJumpHeight;
        Debug.Log("Spring Off");
    }


}
