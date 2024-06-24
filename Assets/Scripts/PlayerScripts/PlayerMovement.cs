using System;
using System.Numerics;
using System.Xml.Schema;
using ScriptableData;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows.Speech;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        private bool _isFacingRight;
        private Vector2 _moveDirection = Vector2.zero;
        private Rigidbody _rb;

        //player's stats. Only Modify 
        [SerializeField] private PlayerStatsSO initialPlayerStats;
        [SerializeField] private PlayerStatsSO currentPlayerStats;

        [Header("Raycast References")] [SerializeField]
        private Vector3 direction = -Vector3.up;

        [SerializeField] private float maxDistance = 1f;
        [SerializeField] private LayerMask groundLayer;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();

            //initializes the value of the boolean depending on the player gameobject's local scale x value
            _isFacingRight = gameObject.transform.localEulerAngles != new Vector3(0, 180, 0);

            //initialize current player stats data using initial player stats

            if (currentPlayerStats == null) return;
            if (initialPlayerStats == null) return;
            currentPlayerStats.movementSpeed = initialPlayerStats.movementSpeed;
            currentPlayerStats.jumpHeight = initialPlayerStats.jumpHeight;
            currentPlayerStats.jumpFallOff = initialPlayerStats.jumpFallOff;
        }

        private void Update()
        {
            Flip();
        }

        private void FixedUpdate()
        {
            //draws a ray for the groundCheck raycast
            Debug.DrawRay(transform.position, direction * maxDistance, Color.yellow);
        }

        //takes the Left and Right input value and uses it for the player's movement
        public void ProcessMove(Vector2 input)
        {
            _moveDirection = input;
            _rb.velocity = new Vector3(_moveDirection.x * currentPlayerStats.movementSpeed, _rb.velocity.y, 0);
        }

        //jump
        public void Jump(InputAction.CallbackContext ctx)
        {
            //checks if the raycast hits an object before jumping
            if (IsGrounded() && ctx.ReadValueAsButton() == true)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, currentPlayerStats.jumpHeight);
            }

            //after jumping button is released, while the player is jumping, drags the player down. 
            else if (ctx.ReadValueAsButton() == false && _rb.velocity.y > 0f)
            {
                _rb.velocity = new Vector3(_rb.velocity.x, _rb.velocity.y * currentPlayerStats.jumpFallOff);
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
            if (_moveDirection.x == 0f) return;
            switch (_moveDirection.x)
            {
                case < 0f:
                {
                    var localRotation = transform.localEulerAngles;
                    localRotation = new Vector3(0, 180, 0);
                    transform.localEulerAngles = localRotation;
                    _isFacingRight = false;
                    break;
                }
                case > 0f:
                {
                    var localRotation = transform.localEulerAngles;
                    localRotation = new Vector3(0, 0, 0);
                    transform.localEulerAngles = localRotation;
                    _isFacingRight = true;
                    break;
                }
            }
        }
        
        //returns True if the player is walking/running, false if not
        public bool IsWalking()
        {
            if (_rb.velocity.y == 0 && _moveDirection.x != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}