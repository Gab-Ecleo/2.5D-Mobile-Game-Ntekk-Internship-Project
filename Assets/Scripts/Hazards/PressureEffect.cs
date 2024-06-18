using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using PlayerScripts;
using ScriptableData;
using Unity.Mathematics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PressureEffect : MonoBehaviour
{
    [SerializeField] private float _hazardDuration = 5f;
    [SerializeField] private PlayerStatsSO currentPlayerStats;

    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private GameObject _player;
    
    [SerializeField] private float _windStr;
    [SerializeField] private Vector3 _windDir;
    private void FixedUpdate()
    {
        // CHANGE TRIGGER in the Future!!! Activates Hazard when button is pressed
        if (Input.GetKeyDown("j"))
        {
            StartCoroutine("windOn");
        }
    }

    //Pushes the player for [Hazard Duration], then returns to the original value
    private IEnumerator windOn()
    {
        
        _playerRB.AddForce (_windStr * _windDir + _player.transform.position );
        Debug.Log("Pushing player :<");
        yield return new WaitForSeconds(_hazardDuration);
        //_playerRB.AddForce (_windStr  - _player.transform.position);
        Debug.Log("Returning :>");
    }
}
