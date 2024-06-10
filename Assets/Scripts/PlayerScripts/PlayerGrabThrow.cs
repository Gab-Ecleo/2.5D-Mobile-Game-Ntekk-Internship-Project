using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerGrabThrow : MonoBehaviour
    {
        [SerializeField] private bool hasItem; //for testing purposes. Delete after testing
        
        [Header("Raycast References")]
        [SerializeField] private LayerMask layerDetected;
        [SerializeField] private Vector3 rayOffset1, rayOffset2;
        [SerializeField] private float rayDistance = 2f;
        private RaycastHit _hit1, _hit2;

        private PlayerGrabCooldown _grabCooldown;

        private void Start()
        {
            _grabCooldown = GetComponent<PlayerGrabCooldown>();
        }

        private void FixedUpdate()
        {
            //Visually projects how the raycasts below would look like
            var position = transform.position;
            Debug.DrawRay(position + rayOffset1, transform.right * rayDistance, Color.blue);
            Debug.DrawRay(position + rayOffset2, transform.right * rayDistance, Color.blue);
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
            if (FirstBlockDetection() != null)
            {
                Debug.Log($"First Raycast Detected Object name:{_hit1.transform.gameObject.name}");
                hasItem = true;
            }
            else if (SecondBlockDetection() != null)
            {
                Debug.Log($"Second Raycast Detected Object name:{_hit2.transform.gameObject.name}");
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

        //returns a gameObject if the raycast has detected the layer based on it. returns null if none
        private GameObject FirstBlockDetection()
        {
            Physics.Raycast(transform.position + rayOffset1, transform.right, out _hit1, rayDistance, layerDetected);
            if (_hit1.collider == null)
            {
                return null;
            }
            return _hit1.collider.gameObject;
        }
        
        //returns a gameObject if the raycast has detected the layer based on it. returns null if none
        private GameObject SecondBlockDetection()
        {
            Physics.Raycast(transform.position + rayOffset2, transform.right, out _hit2, rayDistance, layerDetected);
            if (_hit2.collider == null)
            {
                return null;
            }
            return _hit2.collider.gameObject;
        }
    }
}