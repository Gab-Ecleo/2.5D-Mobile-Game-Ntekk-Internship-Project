using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

public class Spring : MonoBehaviour
{
    private PlayerStatsSO _playerStatsSo;
    private PlayerPowerUps _powerUps;

    void Start()
    {
        _playerStatsSo = Resources.Load("PlayerData/CurrentPlayerStats") as PlayerStatsSO;
        _powerUps = GameObject.FindWithTag("Player").GetComponent<PlayerPowerUps>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerStatsSo.springJump = true;
            _powerUps.PowerUp();
            Destroy(gameObject);
        }
        
        //Box Decay Trigger

        
    }
  
}
