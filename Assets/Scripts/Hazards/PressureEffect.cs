using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using PlayerScripts;
using ScriptableData;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PressureEffect : MonoBehaviour
{
    [SerializeField] private float _hazardDuration = 5f;

    [SerializeField] private Rigidbody _playerRB;
    
    [SerializeField] private float _windStr;
    [SerializeField] private Vector3 _windDir;
    public Vector3 randomVal;


    private void FixedUpdate()
    {
        randomVal = new Vector3(Random.Range(1, 3), 0, 0);
        if (randomVal.x == 1)
        {
            _windDir.x = -5;
        }
        else if (randomVal.x == 2)
        {
            _windDir.x = 5;
        }
        // CHANGE TRIGGER in the Future!!! Activates Hazard when button is pressed
        
        if (Input.GetKeyDown("j"))
        {
            StartCoroutine("windOn");
        }
    }

    // NOT LOOPING YET !! WIP :<
    void windBlow(){
        for (int i = 0; i <= 5; i++)
        {
            _playerRB.AddForce (_windDir * _windStr);
            Debug.Log("Pushing player :<");
        }
        
    }
    
    //Pushes the player for [Hazard Duration], then returns to the original value
    private IEnumerator windOn()
    {
        //windBlow();
        _playerRB.AddForce (_windDir * _windStr);
        Debug.Log("Push push push :>");
        yield return new WaitForSeconds(_hazardDuration);
    }
}
