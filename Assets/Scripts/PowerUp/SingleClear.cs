using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

public class SingleClear : MonoBehaviour
{
    private PlayerStatsSO _playerStatsSo;
    private PlayerPowerUps _powerUps;
    
    void Start()
    {
        _playerStatsSo = Resources.Load("PlayerData/CurrentPlayerStats") as PlayerStatsSO;
        _powerUps = GameObject.FindWithTag("Player").GetComponent<PlayerPowerUps>();
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerStatsSo.singleBlockRemover = true;
            _powerUps.PowerUp();
            Destroy(gameObject);
        }
        
        //Box Decay Trigger
        
    }
}
