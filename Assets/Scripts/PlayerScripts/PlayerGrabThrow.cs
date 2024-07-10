using System;
using AudioScripts;
using AudioScripts.AudioSettings;
using BlockSystemScripts;
using BlockSystemScripts.BlockScripts;
using ScriptableData;
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
        
        private PlayerStatsSO _playerStats;

        private AudioClipsSO _audioClip;
        private AudioManager _audioManager;

        private void Awake()
        {
            InitializeScriptValues();
        }

        private void InitializeScriptValues()
        {
            _playerStats = Resources.Load("PlayerData/CurrentPlayerStats") as PlayerStatsSO;
            _eyeSight = GetComponent<PlayerEyesight>();
            _grabCooldown = GetComponent<PlayerGrabCooldown>();
        }
        
        private void InitializeAudio()
        {
            //initialize current player stats data using initial player stats
            if(_audioClip == null) return;
            _audioManager = AudioManager.Instance;
            _audioClip = _audioManager.FetchAudioClip();
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
                if (_playerStats.singleBlockRemover)
                {
                    DestroySingleBlock(_eyeSight.FirstBlockDetection());
                    return;
                }
                PickUpBlock(_eyeSight.FirstBlockDetection());
            }
            else if (_eyeSight.SecondBlockDetection() != null)
            {
                if (_playerStats.singleBlockRemover)
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
            //Checks if the detected block's type is Heavy
            //If not, checks if it has a block above it on the grid.
            //If there is none, detect if the block can be picked up.
            if (detectedObject.BlockType == BlockType.Heavy) return;
            if (detectedObject.TopBlockDetection() != null) return;
            if (detectedObject.BlockState != BlockState.CanPickUp) return;
            
            //Adds the detected object as the collected block, disables it, then enables the block placeholder 
            collectedBlock = detectedObject;
            collectedBlock.gameObject.SetActive(false);
            blockPlaceholder.SetActive(true);
            
            //Plays the SFX correlating to the action
            SfxScript.Instance.PlaySFXOneShot(_audioClip._pickupSFX);
            
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
            
            //Plays the SFX correlating to the action
            SfxScript.Instance.PlaySFXOneShot(_audioClip._dropSFX);
        }

        //Triggered if player has the Single Block Clear Powerup
        private void DestroySingleBlock(BlockScript detectedBlock)
        {
            if (detectedBlock.BlockState != BlockState.CanPickUp) return;
            Destroy(detectedBlock.gameObject);
            _playerStats.singleBlockRemover = false;
        }
        #endregion
    }
}