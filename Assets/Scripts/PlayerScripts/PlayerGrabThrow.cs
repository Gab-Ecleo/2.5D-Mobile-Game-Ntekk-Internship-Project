using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerGrabThrow : MonoBehaviour
    {
        [SerializeField] private bool hasItem; //for testing purposes. Delete after testing
        private PlayerGrabCooldown _grabCooldown;
        private PlayerEyesight _eyeSight;

        private void Start()
        {
            _eyeSight = GetComponent<PlayerEyesight>();
            _grabCooldown = GetComponent<PlayerGrabCooldown>();
        }

        public void Interact(InputAction.CallbackContext ctx)
        {
            //if grab is on cooldown, ignore this function. 
            if (_grabCooldown.GrabCoolDownEnabled()) return;
            
            //if player is carrying an item, Throw item, then ignore the rest
            if (hasItem)
            {
                Throw();
                return;
            }
            
            //detects object in front of player; eyesight level first, then waist level
            if (_eyeSight.FirstBlockDetection() != null)
            {
                Debug.Log($"First Raycast Detected Object name:{_eyeSight.FirstBlockDetection().name}");
                hasItem = true;
            }
            else if (_eyeSight.SecondBlockDetection() != null)
            {
                Debug.Log($"Second Raycast Detected Object name:{_eyeSight.FirstBlockDetection().name}");
                hasItem = true;
            }
            else
            {
                Debug.Log("No Object has been detected by either raycasts");
            }
        }
        
        private void Throw()
        {
            _grabCooldown.StartTimer();
            hasItem = false;
            Debug.Log("Player has Thrown");
        }
    }
}