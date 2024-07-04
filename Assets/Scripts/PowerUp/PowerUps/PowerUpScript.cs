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
            PlayerStatsSo = Resources.Load("PlayerData/CurrentPlayerStats") as PlayerStatsSO;
            _powerUps = GameObject.FindWithTag("Player").GetComponent<PlayerPowerUps>();
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _powerUps.PowerUp();
                Destroy(gameObject);
            }
        }
    }
}