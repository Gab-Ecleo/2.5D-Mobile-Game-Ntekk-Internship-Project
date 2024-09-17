using BlockSystemScripts;
using BlockSystemScripts.BlockScripts;
using EventScripts;
using ScriptableData;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerGrabThrow : MonoBehaviour
    {
        [Header("Collected Block Placeholder")]
        [SerializeField] private BlockPlaceholder blockPlaceholder;
        
        [Header("Test References. To be private")]
        [SerializeField] private BlockScript collectedBlock;//for testing purposes. Unserialize after testing
        [SerializeField] private bool hasItem; //for testing purposes. Unserialize after testing

        public bool HasItem => hasItem;

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
                if (GameManager.Instance.FetchPowerUps().singleBlockRemover)
                {
                    DestroySingleBlock(_eyeSight.FirstBlockDetection());
                    return;
                }
                PickUpBlock(_eyeSight.FirstBlockDetection());
            }
            else if (_eyeSight.SecondBlockDetection() != null)
            {
                if (GameManager.Instance.FetchPowerUps().singleBlockRemover)
                {
                    DestroySingleBlock(_eyeSight.SecondBlockDetection());
                    return;
                }
                PickUpBlock(_eyeSight.SecondBlockDetection());
            }
            else
            {
                Debug.Log("No Object has been detected by either raycasts");
            }
        }

        private void PickUpBlock(BlockScript detectedObject)
        {
            //Checks if the detected block's type is Heavy or is a Power Up
            //If not, checks if it has a block above it on the grid.
            //If there is none, detect if the block can be picked up.
            if (detectedObject.BlockType is BlockType.Heavy or BlockType.PowerUp) return;
            if (detectedObject.TopBlockDetection() != null) return;
            if (detectedObject.BlockState != BlockState.CanPickUp) return;
            
            //Adds the detected object as the collected block, disables it, then enables the block placeholder 
            collectedBlock = detectedObject;
            collectedBlock.gameObject.SetActive(false);
            blockPlaceholder.ToggleActive(collectedBlock);
            hasItem = true;
            
            //Plays events correlating to the action
            AudioEvents.ON_PLAYER_PICKUP?.Invoke();
            PlayerEvents.ON_PLAYER_PICKUP?.Invoke();
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
            blockPlaceholder.ToggleActive(null);
            _grabCooldown.StartTimer();
            hasItem = false;
            
            //nullifies the values of the collected block
            collectedBlock = null;
            //Debug.Log("Player has Thrown");
            
            //Plays the events correlating to the action
            AudioEvents.ON_PLAYER_DROP?.Invoke();
            PlayerEvents.ON_PLAYER_DROP?.Invoke();
        }

        //Triggered if player has the Single Block Clear Powerup
        private void DestroySingleBlock(BlockScript detectedBlock)
        {
            if (detectedBlock.BlockType == BlockType.PowerUp) return;
            if (detectedBlock.BlockState != BlockState.CanPickUp) return;
            Destroy(detectedBlock.gameObject);
            PowerUpsEvents.DEACTIVATE_SINGLECLEAR_PU?.Invoke();
        }
        #endregion
    }
}