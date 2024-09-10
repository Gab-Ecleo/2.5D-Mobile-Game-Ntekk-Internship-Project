using System;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace PowerUp.PowerUps
{
    public class PowerUpScript : MonoBehaviour
    {
        private protected PlayerStatsSO PlayerStatsSo;
        private PlayerPowerUps _powerUps;

        private void Start()
        {
            PlayerStatsSo = GameManager.Instance.FetchCurrentPlayerStat();
            _powerUps = GameManager.Instance.FetchPlayer().GetComponent<PlayerPowerUps>();
        }

        protected void BaseEffect()
        {
            _powerUps.PowerUp();
            Destroy(gameObject);
        }
        
    }
}