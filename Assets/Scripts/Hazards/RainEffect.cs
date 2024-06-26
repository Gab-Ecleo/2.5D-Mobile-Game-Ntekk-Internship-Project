using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using ScriptableData;
using UnityEngine;
using UnityEngine.Serialization;

public class RainEffect : MonoBehaviour
{
    [SerializeField] private float hazardDuration = 5f;
    [SerializeField] private PlayerStatsSO currentPlayerStats;
    [SerializeField] private float currentSpeed;
    [SerializeField] private float hazardModifier = 2;

    private void Start()
    {
        //Set current speed from the player stat speed value
        currentSpeed = currentPlayerStats.movementSpeed;
    }

    private void Update()
    {
        // CHANGE TRIGGER in the Future!!! Activates Hazard when button is pressed
        if (Input.GetKeyDown("l"))
        {
            StartCoroutine(nameof(rainOn));
        }
    }
    
    //Halves the player's speed for [Hazard Duration], then returns to the original value
    IEnumerator rainOn()
    {
        currentSpeed /= hazardModifier;
        currentPlayerStats.movementSpeed = currentSpeed;
        Debug.Log("Slow down player :<");
        yield return new WaitForSeconds(hazardDuration);
        currentSpeed *= hazardModifier;
        currentPlayerStats.movementSpeed = currentSpeed;
        Debug.Log("Speed up Player :>");
    }
}
