using System;
using BlockSystemScripts.BlockScripts;
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
            //If the triggered game object is a powerup, the player does not get damages
            if (other.GetComponent<BlockScript>().BlockType == BlockType.PowerUp) return;
            PlayerEvents.OnPlayerDamage?.Invoke();
            Destroy(other.gameObject); //for testing only. delete after testing. 
        }
    }
}