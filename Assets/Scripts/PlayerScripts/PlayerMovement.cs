using System;
using System.Numerics;
using System.Xml.Schema;
using AudioScripts;
using AudioScripts.AudioSettings;
using ScriptableData;
//using Unity.PlasticSCM.Editor.WebApi;
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
        private AudioClipsSO _audioClip;
        private AudioManager _audioManager;
        
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
        #endregion
        
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
            currentPlayerStats.deceleration = initialPlayerStats.deceleration;
            currentPlayerStats.velPower = initialPlayerStats.velPower;
            currentPlayerStats.frictionAmount = initialPlayerStats.frictionAmount;
            
            currentPlayerStats.jumpHeight = initialPlayerStats.jumpHeight;
            currentPlayerStats.jumpCutMultiplier = initialPlayerStats.jumpCutMultiplier;
        }
        
        private void InitializeAudio()
        {
            //initialize current player stats data using initial player stats
            if(_audioClip == null) return;
            _audioManager = AudioManager.Instance;
            _audioClip = _audioManager.FetchAudioClip();
        }
        #endregion

        #region MOVEMENT_CALCULATIONS
         //takes the Left and Right input value and uses it for the player's movement
        public void ProcessMove(Vector2 input)
        {
            #region MOVEMENT
            _moveDirection = input;

            //calculated the player's input multiplied by the set movement speed
            float targetSpeed = _moveDirection.x * currentPlayerStats.movementSpeed;
            //calculates the difference between the set target speed and the player's current velocity
            float speedDiff = targetSpeed - _rb.velocity.x;
            //checks if the target speed is more than or less than 0, allowing ato switch between the set acceleration and set deceleration
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? currentPlayerStats.acceleration : currentPlayerStats.deceleration;
            //calculates the final movement computation
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, currentPlayerStats.velPower) *
                                 Mathf.Sign(speedDiff);

            //determines which type of aerial movement implementation will the player use
            switch (currentPlayerStats.movementState)
            {
                case PlayerMovementState.WithAerialMovement:
                    _rb.AddForce(movement * Vector2.right);
                    break;
                case PlayerMovementState.ReducedAerialMovement:
                {
                    _rb.AddForce(movement * Vector2.right);
                    if (!IsGrounded())
                    {
                        var velocity = _rb.velocity;
                        velocity = new Vector3(velocity.x * currentPlayerStats.aerialSpdReducer, velocity.y,
                            velocity.z);
                        _rb.velocity = velocity;
                    }
                    break;
                }
                case PlayerMovementState.ReducedFlippedMovement:
                {
                    if (!IsGrounded())
                    {
                        _rb.AddForce(movement * Vector2.right * currentPlayerStats.aerialSpdReducer);
                        
                    }
                    else if (IsGrounded())
                    {
                        _rb.AddForce(movement * Vector2.right);
                    }
                    break;
                }
                case PlayerMovementState.NoAerialMovement:
                {
                    if (IsGrounded())
                    {
                        _rb.AddForce(movement * Vector2.right);
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
                float force = currentPlayerStats.jumpHeight;
                if (_rb.velocity.y<0)
                {
                    force -= _rb.velocity.y;
                }
                _rb.AddForce(Vector2.up*force, ForceMode.Impulse);
                
                // Plays SFX correlating to the action
                SfxScript.Instance.PlaySFXOneShot(_audioClip._jumpSFX);
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