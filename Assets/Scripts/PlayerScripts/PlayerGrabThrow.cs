using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerGrabThrow : MonoBehaviour
    {
        private RaycastHit _hit1, _hit2;
        [SerializeField] private LayerMask layerDetected;
        [SerializeField] private Vector3 rayOffset1, rayOffset2;
        [SerializeField] private float rayDistance = 2f;
        private void FixedUpdate()
        {
            //Visually projects how the raycasts below would look like
            var position = transform.position;
            Debug.DrawRay(position + rayOffset1, transform.right * rayDistance, Color.blue);
            Debug.DrawRay(position + rayOffset2, transform.right * rayDistance, Color.blue);
        }

        public void Interact(InputAction.CallbackContext ctx)
        {
            if (FirstBlockDetection() != null)
            {
                Debug.Log($"First Raycast Detected Object name:{_hit1.transform.gameObject.name}");
            }
            else if (SecondBlockDetection() != null)
            {
                Debug.Log($"Second Raycast Detected Object name:{_hit2.transform.gameObject.name}");
            }
            else
            {
                Debug.Log("No Object has been detected by either raycasts");
            }
        }
        
        private void Throw()
        {
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