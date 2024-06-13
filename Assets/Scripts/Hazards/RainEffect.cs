using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using ScriptableData;
using UnityEngine;

public class RainEffect : MonoBehaviour
{
    [SerializeField] private float _hazardDuration = 5f;
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private float _hazardModifier = 2;

    private void Start()
    {
        //Set current speed from the player stat speed value
        _currentSpeed = _player.Speed;
    }

    private void Update()
    {
        // CHANGE TRIGGER in the Future!!! Activates Hazard when button is pressed
        if (Input.GetKeyDown("l"))
        {
            StartCoroutine("rainOn");
        }
    }
    
    //Halves the player's speed for [Hazard Duration], then returns to the original value
    IEnumerator rainOn()
    {
        _currentSpeed /= _hazardModifier;
        _player.Speed = _currentSpeed;
        Debug.Log("Slow down player :<");
        yield return new WaitForSeconds(_hazardDuration);
        _currentSpeed *= _hazardModifier;
        _player.Speed = _currentSpeed;
        Debug.Log("Speed up Player :>");
    }
}
