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
            if (_eyesight.GridCellDetection() == null)
            {
                TurnOffBoth();
                return;
            }
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
            if (_eyesight.GridCellDetection() != null)
            {
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
        }

        private void NoBlockIndicators()
        {
            if (_eyesight.FirstBlockDetection() != null)
            {
                TriggerNoBlockIndicators( _eyesight.FirstBlockDetection());
            }
            else if (_eyesight.SecondBlockDetection() != null)
            {
                TriggerNoBlockIndicators( _eyesight.SecondBlockDetection());
            }
            else
            {
                TurnOffBoth();
            }
        }

        private void TriggerNoBlockIndicators(BlockScript detectedObject)
        {
            if (detectedObject.BlockType is BlockType.Heavy or BlockType.PowerUp) { TurnOffBoth(); return; }
            if (detectedObject.TopBlockDetection() != null) { TurnOffBoth(); return; }
            if (detectedObject.BlockState != BlockState.CanPickUp) { TurnOffBoth(); return; }

            ToggleToValid();
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