using System;
using BlockSystemScripts;
using BlockSystemScripts.BlockScripts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerGrabThrow : MonoBehaviour
    {
        [Header("Collected Block Placeholder")]
        [SerializeField] private GameObject blockPlaceholder;
        
        [Header("Test References. To be private")]
        [SerializeField] private BlockScript collectedBlock;//for testing purposes. Unserialize after testing
        [SerializeField] private bool hasItem; //for testing purposes. Unserialize after testing
        
        private PlayerGrabCooldown _grabCooldown;
        private PlayerEyesight _eyeSight;

        private void Awake()
        {
            InitializeScriptValues();
        }

        private void InitializeScriptValues()
        {
            _eyeSight = GetComponent<PlayerEyesight>();
            _grabCooldown = GetComponent<PlayerGrabCooldown>();
        }

        #region PLAYERACTION_METHODS
        public void Interact(InputAction.CallbackContext ctx)
        {
            //if grab is on cooldown, ignore this function. 
            if (_grabCooldown.GrabCoolDownEnabled()) return;
            
            //if player is carrying an item, Throw item, then ignore the rest
            if (hasItem)
            {
                ThrowBlock(_eyeSight.GridCellDetection());
                return;
            }
            
            //detects object in front of player; eyesight level first, then waist level
            if (_eyeSight.FirstBlockDetection() != null)
            {
                PickUpBlock(_eyeSight.FirstBlockDetection());
            }
            else if (_eyeSight.SecondBlockDetection() != null)
            {
                PickUpBlock(_eyeSight.SecondBlockDetection());
            }
            else
            {
                Debug.Log("No Object has been detected by either raycasts");
            }
        }

        private void PickUpBlock(BlockScript detectedObject)
        {
            //Checks if the detected block has a block above it on the grid.
            //If there is none, detect if the object can be picked up.
            if (detectedObject.TopBlockDetection() != null) return;
            if (!detectedObject.CanPickUp) return;
            
            //Adds the detected object as the collected block, disables it, then enables the block placeholder 
            collectedBlock = detectedObject;
            collectedBlock.gameObject.SetActive(false);
            blockPlaceholder.SetActive(true);
                
            hasItem = true;
        }

        private void ThrowBlock(GridCell detectedCell)
        {
            //Checks if there is a detected cell
            //If there is a detected cell, check if the cell has no current block in it.
            if (detectedCell == null)
            {
                Debug.Log("NO GRID CELL DETECTED"); 
                return;
            }
            if (detectedCell.CurrentBlock!= null) return;
            
            //enables the disabled game object and give it the cell's position, then gives it's references
            //disable the column's spawning
            collectedBlock.gameObject.SetActive(true);
            collectedBlock.transform.position = detectedCell.transform.position;
            collectedBlock.InitializeReferences(detectedCell, detectedCell.AssignedSpawner);
            detectedCell.AssignedSpawner.TriggerCannotSpawn();
            
            //disable the placeholder, then start the grab cooldown timer
            blockPlaceholder.SetActive(false);
            _grabCooldown.StartTimer();
            hasItem = false;
            
            //nullifies the values of the collected block
            collectedBlock = null;
            //Debug.Log("Player has Thrown");
        }
        #endregion
    }
}