using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [Header("Spring Power-Up")]
    [SerializeField] protected float SpringJumpHeight;
    
    private PowerUpManager _powerUp;

    void Start()
    {
        _powerUp = GameObject.FindWithTag("PowerUp Manager").GetComponent<PowerUpManager>();
        StartCoroutine(_powerUp.Decay());
    }

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(_powerUp.SpringPower());
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }
  
}
