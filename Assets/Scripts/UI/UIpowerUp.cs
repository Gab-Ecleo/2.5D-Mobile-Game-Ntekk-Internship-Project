using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptableData;

public class UIpowerUp : MonoBehaviour
{
    // for testing purposes only
    [SerializeField] private PlayerStatsSO _playerCurrStats;
    [SerializeField] private TMP_Text _powerupText;
    void Update()
    {
        if(_playerCurrStats != null)
        {
            if (_playerCurrStats.hasMultiplier)
            {
                _powerupText.text = "Multiplier";
            }
            else if(_playerCurrStats.springJump)
            {
                _powerupText.text = "Spring Jump";
            }
            else if(_playerCurrStats.timeSlow)
            {
                _powerupText.text = "Time Slow";
            }
            else if (_playerCurrStats.expressDelivery)
            {
                _powerupText.text = "Express Delivery";
            }
            else if(_playerCurrStats.singleBlockRemover)
            {
                _powerupText.text = "Single Block";
            }
            else
            {
                _powerupText.text = "Power-up";
            }
        }
    }
}
