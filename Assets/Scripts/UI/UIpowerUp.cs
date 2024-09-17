using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptableData;
using UnityEngine.UI;

public class UIpowerUp : MonoBehaviour
{
    // for testing purposes only
    [SerializeField] private PlayerStatsSO _playerCurrStats;
    [SerializeField] private TMP_Text _powerupText;
    void Update()
    {
        if(_playerCurrStats != null)
        {
            if (GameManager.Instance.FetchPowerUps().hasMultiplier)
            {
                _powerupText.text = "Multiplier";
            }
            else if(GameManager.Instance.FetchPowerUps().springJump)
            {
                _powerupText.text = "Spring Jump";
            }
            else if(GameManager.Instance.FetchPowerUps().timeSlow)
            {
                _powerupText.text = "Time Slow";
            }
            else if (GameManager.Instance.FetchPowerUps().expressDelivery)
            {
                _powerupText.text = "Express Delivery";
            }
            else if(GameManager.Instance.FetchPowerUps().singleBlockRemover)
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
