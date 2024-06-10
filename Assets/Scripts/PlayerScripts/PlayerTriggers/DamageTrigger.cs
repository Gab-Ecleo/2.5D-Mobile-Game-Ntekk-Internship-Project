using System;
using Events;
using UnityEngine;

namespace PlayerScripts.PlayerTriggers
{
    //Script for the player's damage collider
    public class DamageTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Block")) return;
            PlayerEvents.OnPlayerDamageMethod();    
            Destroy(other.gameObject); //for testing only. delete after testing. 
        }
    }
}