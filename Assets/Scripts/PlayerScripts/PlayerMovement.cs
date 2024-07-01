using System;
using System.Numerics;
using System.Xml.Schema;
using ScriptableData;
using Unity.PlasticSCM.Editor.WebApi;
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

        private void Awake()
        {
            InitializeScriptValues();
        }

        private void Start()
        {
           InitializePlayerStats();
        }

        private void Update()
        {
            Flip();
            
        }
        
        private void FixedUpdate() //For Testing. Can be Deleted
        {
            //draws a ray for the groundCheck raycast
            Debug.DrawRay(transform.position, direction * maxDistance, Color.yellow);
        }

        #region INITIALIZATION_METHODS
        private void InitializeScriptValues()
        {
            _rb = GetComponent<Rigidbody>();

            //initializes the value of the boolean depending on the player gameobject's local scale x value
            _isFacingRight = gameObject.transform.localEulerAngles != new Vector3(0, 180, 0);
        }

        private void InitializePlayerStats()
        {
            //initialize current player stats data using initial player stats
            if (currentPlayerStats == null) return;
            if (initialPlayerStats == null) return;
            currentPlayerStats.movementSpeed = initialPlayerStats.movementSpeed;
            currentPlayerStats.acceleration = initialPlayerStats.acceleration;
            currentPlayerStats.decceleration = initialPlayerStats.decceleration;
            currentPlayerStats.velPower = initialPlayerStats.velPower;
            currentPlayerStats.frictionAmount = initialPlayerStats.frictionAmount;
            
            currentPlayerStats.jumpHeight = initialPlayerStats.jumpHeight;
            currentPlayerStats.jumpCutMultiplier = initialPlayerStats.jumpCutMultiplier;
        }
        #endregion

        #region MOVEMENT_CALCULATIONS
         //takes the Left and Right input value and uses it for the player's movement
        public void ProcessMove(Vector2 input)
        {
            #region MOVEMENT
            _moveDirection = input;
            //_rb.velocity = new Vector3(_moveDirection.x * currentPlayerStats.movementSpeed, _rb.velocity.y, 0);

            float targetSpeed = _moveDirection.x * currentPlayerStats.movementSpeed;
            float speedDiff = targetSpeed - _rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? currentPlayerStats.acceleration : currentPlayerStats.decceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, currentPlayerStats.velPower) *
                             Mathf.Sign(speedDiff);
            
            _rb.AddForce(movement *Vector2.right);
            #endregion

            #region FRICTION
            if (IsGrounded() && Mathf.Abs(_moveDirection.x) <= 0)
            {
                float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(currentPlayerStats.frictionAmount));
                amount *= Mathf.Sign(_rb.velocity.x);
                _rb.AddForce(Vector2.right*-amount, ForceMode.Impulse);
            }
            #endregion
        }

        //jump
        public void Jump(InputAction.CallbackContext ctx)
        {
            //checks if the raycast hits an object before jumping
            if (IsGrounded() && ctx.ReadValueAsButton() == true)
            {
                //_rb.velocity = new Vector3(_rb.velocity.x, currentPlayerStats.jumpHeight);
                float force = currentPlayerStats.jumpHeight;
                if (_rb.velocity.y<0)
                {
                    force -= _rb.velocity.y;
                }
                _rb.AddForce(Vector2.up*force, ForceMode.Impulse);
            }

            //after jumping button is released, while the player is jumping, drags the player down. 
            else if (ctx.ReadValueAsButton() == false && _rb.velocity.y > 0f)
            {
                _rb.AddForce(Vector3.down * _rb.velocity.y*(1-currentPlayerStats.jumpCutMultiplier), ForceMode.Impulse);
            }
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
        
        //returns True if raycast has detected groundLayer.
        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, direction, maxDistance, groundLayer);
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
        #endregion
    }
}