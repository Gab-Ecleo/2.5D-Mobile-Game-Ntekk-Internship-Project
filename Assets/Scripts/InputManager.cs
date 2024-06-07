﻿using System;
using PlayerScripts;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{ 
        private PlayerMovement _playerMovement; 
        private PlayerGrabThrow _playerGrabThrow;

        private PlayerControls _playerControls;
        private InputAction _move;
        private InputAction _jump;
        private InputAction _interactObject;

        //initialize values
        private void Awake()
        {
                _playerControls = new PlayerControls();
                _playerMovement = GetComponent<PlayerMovement>();
                _playerGrabThrow = GetComponent<PlayerGrabThrow>();
        }

        
        private void FixedUpdate()
        {
                //forwards the values of the player's movement inputs to the movement script
                _playerMovement.ProcessMove(_move.ReadValue<Vector2>());
        }

        private void OnEnable()
        {
                _move = _playerControls.Player.Move;
                _move.Enable();

                _jump = _playerControls.Player.Jump;
                _jump.Enable();
                _jump.performed += _playerMovement.Jump;
                _jump.canceled += _playerMovement.Jump; //triggered by the release of the jump button

                _interactObject = _playerControls.Player.Interact;
                _interactObject.Enable();
                _interactObject.performed += _playerGrabThrow.Interact;
        }

        private void OnDisable()
        {
                _move.Disable();
                _jump.Disable();
                _interactObject.Disable();
        }
}
