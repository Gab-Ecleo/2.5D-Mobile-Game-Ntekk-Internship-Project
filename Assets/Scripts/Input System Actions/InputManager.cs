using PlayerScripts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input_System_Actions
{
    public class InputManager : MonoBehaviour
    {
        private PlayerMovement _playerMovement;
        private PlayerGrabThrow _playerGrabThrow;
        private PlayerPowerUps _playerPowerUps;

        private PlayerControls _playerControls;
        private InputAction _move;
        private InputAction _jump;
        private InputAction _interactObject;

        private InputAction _interactPowerUp;
        
        //initialize values
        private void Awake()
        {
            InitializeScriptValues();
        }

        private void FixedUpdate()
        {
            //forwards the values of the player's movement inputs to the movement script
            _playerMovement.ProcessMove(_move.ReadValue<Vector2>());
        }

        private void InitializeScriptValues()
        {
            _playerControls = new PlayerControls();
            _playerMovement = GetComponent<PlayerMovement>();
            _playerGrabThrow = GetComponent<PlayerGrabThrow>();
            _playerPowerUps = GetComponent<PlayerPowerUps>();
        }

        //enable the listeners for the buttons
        private void OnEnable()
        {
            _move = _playerControls.Player.Move;
            _move.Enable();

            _jump = _playerControls.Player.Jump;
            _jump.Enable();
            _jump.performed += _playerMovement.Jump; //triggered by pressing the jump button
            _jump.canceled += _playerMovement.Jump; //triggered by releasing the jump button

            _interactObject = _playerControls.Player.Interact;
            _interactObject.Enable();
            _interactObject.performed += _playerGrabThrow.Interact;

            _interactPowerUp = _playerControls.Player.PowerUp;
            _interactPowerUp.Enable();
            _interactPowerUp.performed += _playerPowerUps.PowerUp;
        }

        //disable the listeners for the buttons
        private void OnDisable()
        {
            _move.Disable();
            _jump.Disable();
            _interactObject.Disable();
            _interactPowerUp.Disable();
        }
    }
}
