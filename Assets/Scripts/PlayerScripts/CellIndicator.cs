using System;
using BlockSystemScripts;
using BlockSystemScripts.BlockScripts;
using UnityEngine;

namespace PlayerScripts
{
    public class CellIndicator : MonoBehaviour
    {
        [Header("Indicator References")] 
        [SerializeField] private GameObject validIndicator;
        [SerializeField] private GameObject invalidIndicator;

        private PlayerEyesight _eyesight;
        private PlayerGrabThrow _grabThrow;

        private GridCell _lastGridCell;
        private BlockScript _lastBlockScript;
        
        private void Awake()
        {
            _eyesight = GetComponent<PlayerEyesight>();
            _grabThrow = GetComponent<PlayerGrabThrow>();
        }

        private void Update()
        {
            RenderIndicators();
        }
        
        private void RenderIndicators()
        {
            //Checks if no grid cells are detected in the player's sight
            if (_eyesight.GridCellDetection() == null)
            {
                TurnOffBoth();
                return;
            }
            
            //Checks if the player is currently holding a block or no;
            if (_grabThrow.HasItem)
            {
                HasBlockIndicators();
            }
            else
            {
                NoBlockIndicators();
            }
        }
        
        private void HasBlockIndicators()
        {
            // Checks if the detected gridcell contains a block
            if (_eyesight.GridCellDetection().CurrentBlock != null)
            {
                ToggleToInvalid();
                    
                if (invalidIndicator.transform.position == _eyesight.GridCellDetection().transform.position) return;
                invalidIndicator.transform.position = _eyesight.GridCellDetection().transform.position;
            }
            else
            {
                ToggleToValid();
                    
                if (validIndicator.transform.position == _eyesight.GridCellDetection().transform.position) return;
                validIndicator.transform.position = _eyesight.GridCellDetection().transform.position;
            }
        }

        private void NoBlockIndicators()
        {
            //Checks if the eye-level ray has detected a block
            if (_eyesight.FirstBlockDetection() != null)
            {
                TriggerNoBlockIndicators( _eyesight.FirstBlockDetection());
            }
            
            //... If not, checks if the waist-level ray has detected a block
            else if (_eyesight.SecondBlockDetection() != null)
            {
                TriggerNoBlockIndicators( _eyesight.SecondBlockDetection());
            }
            
            //... If not too, turn off both indicators
            else
            {
                TurnOffBoth();
            }
        }

        private void TriggerNoBlockIndicators(BlockScript detectedObject)
        {
            //check these conditions before actually rendering the indicator
            if (detectedObject.BlockType is BlockType.Heavy or BlockType.PowerUp) { TurnOffBoth(); return; }
            if (detectedObject.TopBlockDetection() != null) { TurnOffBoth(); return; }
            if (detectedObject.BlockState != BlockState.CanPickUp) { TurnOffBoth(); return; }

            ToggleToValid();
            
            if (validIndicator.transform.position == detectedObject.transform.position) return;
            validIndicator.transform.position = detectedObject.transform.position;
        }

        #region ITEM_TOGGLES
        private void ToggleToValid()
        {
            if (validIndicator.activeSelf == true && invalidIndicator.activeSelf == false) return;
            validIndicator.SetActive(true);
            invalidIndicator.SetActive(false);
        }

        private void ToggleToInvalid()
        {
            if (validIndicator.activeSelf == false && invalidIndicator.activeSelf == true) return;
            validIndicator.SetActive(false);
            invalidIndicator.SetActive(true);
        }

        private void TurnOffBoth()
        {
            if (validIndicator.activeSelf == false && invalidIndicator.activeSelf == false) return;
            validIndicator.SetActive(false);
            invalidIndicator.SetActive(false);
        }
        #endregion
    }
}