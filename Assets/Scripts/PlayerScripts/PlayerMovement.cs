using System;
using AudioScripts.AudioSettings;
using EventScripts;
using Player_Statistics;
using ScriptableData;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace PlayerScripts
{
    public class PlayerMovement : MonoBehaviour
    {
        private bool _isFacingRight;
        private Vector2 _moveDirection = Vector2.zero;
        private Rigidbody _rb;
        
        [Header("Movement Implementation Choice")]
        public PlayerMovementState movementState = PlayerMovementState.ReducedFlippedMovement;

        [Header("Player Data References")]
        //player's stats. Only Modify 
        [SerializeField] private PlayerStatsSO initialPlayerStats;
        [SerializeField] private PlayerStatsSO currentPlayerStats;
        
        [Header("Raycast References")] 
        [SerializeField] private Vector3 direction = -Vector3.up;

        [SerializeField] private float maxDistance = 1f;
        [SerializeField] private LayerMask groundLayer;

        #region UNITY_DEFAULT_FUNCTIONS
        private void Awake()
        {
            InitializeScriptValues();
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
        #endregion
        
        #region INITIALIZATION_METHODS
        private void InitializeScriptValues()
        {
            _rb = GetComponent<Rigidbody>();

            //initializes the value of the boolean depending on the player gameobject's local scale x value
            _isFacingRight = gameObject.transform.localEulerAngles != new Vector3(0, 180, 0);
        }
        
        #endregion

        #region MOVEMENT_CALCULATIONS
         //takes the Left and Right input value and uses it for the player's movement
        public void ProcessMove(Vector2 input)
        {
            #region MOVEMENT
            _moveDirection = input;

            //calculated the player's input multiplied by the set movement speed
            float targetSpeed = _moveDirection.x * currentPlayerStats.stats.movementSpeed;
            //calculates the difference between the set target speed and the player's current velocity
            float speedDiff = targetSpeed - _rb.velocity.x;
            //checks if the target speed is more than or less than 0, allowing ato switch between the set acceleration and set deceleration
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? currentPlayerStats.stats.acceleration : currentPlayerStats.stats.deceleration;
            //calculates the final movement computation
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, currentPlayerStats.stats.velPower) *
                                 Mathf.Sign(speedDiff);

            if (!IsGrounded())
            {
                _rb.AddForce(movement * Vector2.right * currentPlayerStats.stats.aerialSpdReducer);
                        
            }
            else if (IsGrounded())
            {
                _rb.AddForce(movement * Vector2.right);
            }
            #endregion
        }

        //jump
        public void Jump(InputAction.CallbackContext ctx)
        {
            //checks if the raycast hits an object before jumping
            if (IsGrounded() && ctx.ReadValueAsButton())
            {
                // Plays SFX correlating to the action
                AudioEvents.ON_PLAYER_JUMP?.Invoke();

                #region Jump Calculation

                float force = currentPlayerStats.stats.jumpHeight;
                if (_rb.velocity.y<0)
                {
                    force -= _rb.velocity.y;
                }
                _rb.AddForce(Vector2.up*force, ForceMode.Impulse);

                #endregion
            }

            //after jumping button is released, while the player is jumping, drags the player down. 
            else if (ctx.ReadValueAsButton() == false && _rb.velocity.y > 0f)
            {
                _rb.AddForce(Vector3.down * _rb.velocity.y*(1-currentPlayerStats.stats.jumpCutMultiplier), ForceMode.Impulse);
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
            return _rb.velocity.y == 0 && _moveDirection.x != 0;
        }
        #endregion
    }
}