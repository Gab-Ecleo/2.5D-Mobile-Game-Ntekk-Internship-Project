using System;
using System.Numerics;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows.Speech;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        // private float _horizontal;
        private bool _isFacingRight;
        private Vector2 moveDirection = Vector2.zero;

        [Header("Movement Stats")] 
        [SerializeField] private float speed = 8f;
        [SerializeField] private float jumpingPower = 8f;
        [SerializeField] private float fallOffRate = 0.7f;

        [Header("Player References")] 
        [SerializeField] private Rigidbody rb;
        // [SerializeField] private Transform grounder;
        
        [Header("Raycast References")]
        [SerializeField] private Vector3 direction = -Vector3.up;
        [SerializeField] private float maxDistance = 1f;
        [SerializeField] private LayerMask groundLayer;

        private void Start()
        {
            //initializes the value of the boolean depending on the player gameobject's local scale x value
            _isFacingRight = gameObject.transform.localScale.x != -1;
        }

        private void Update()
        {
            Flip();
        }

        private void FixedUpdate()
        {
            //draws a ray for the groundCheck raycast
            Debug.DrawRay(transform.position, direction*maxDistance, Color.yellow);
        }

        //takes the Left and Right input value and uses it for the player's movement
        public void ProcessMove(Vector2 input)
        {
            moveDirection = input;
            rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y);
        }

        //jump
        public void Jump(InputAction.CallbackContext ctx)
        {
            //checks if the raycast hits an object before jumping
            if (IsGrounded() && ctx.ReadValueAsButton() == true)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpingPower);
            }

            //after jumping button is released, while the player is jumping, drags the player down. 
            else if (ctx.ReadValueAsButton() == false && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * fallOffRate);
            }
        }
        
        //returns True if raycast has detected groundLayer.
        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, direction, maxDistance, groundLayer);
        }

        //flips the player's "facing" direction
        private void Flip()
        {
            if (_isFacingRight && moveDirection.x < 0f || !_isFacingRight && moveDirection.x > 0f) 
            {
                _isFacingRight = !_isFacingRight;
                var localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }
        }
    }
}
