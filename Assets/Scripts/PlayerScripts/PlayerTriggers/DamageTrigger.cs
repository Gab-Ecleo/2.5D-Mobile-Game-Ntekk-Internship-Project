using System;
using EventScripts;
using UnityEngine;

namespace PlayerScripts.PlayerTriggers
{
    //Script for the player's damage collider
    public class DamageTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Block")) return;
            PlayerEvents.OnPlayerDamage?.Invoke();
            Destroy(other.gameObject); //for testing only. delete after testing. 
        }
    }
}