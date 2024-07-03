using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

public class ScoreMulti : MonoBehaviour
{
    private PlayerStatsSO _playerStatsSo;
    private PlayerPowerUps _powerUps;

    // Start is called before the first frame update
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
            _playerStatsSo.hasMultiplier = true;
            _powerUps.PowerUp();
            Destroy(gameObject);
        }
    }
}
