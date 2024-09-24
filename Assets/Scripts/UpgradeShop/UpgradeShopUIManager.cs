using System.Collections;
using System.Collections.Generic;
using EventScripts;
using ScriptableData;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace UpgradeShop
{
    public class UpgradeShopUIManager : MonoBehaviour
    {
        [Header("Insufficient Funds UI")] 
        [SerializeField] private GameObject noMoneyUI;
        [SerializeField] private float uiTimer = 1f;

        [Header("Confirm Button")] 
        [SerializeField] private List<int> itemLevelCounter;
        [SerializeField] private Button confirmButton;
        private int _listPosition = 0;
        private bool _noChanges = true;

        private bool _isCoroutineActive;
        public bool NoChanges => _noChanges;

        #region INSUFFICIENT_FUNDS
        private void TriggerNoMoneyUI()
        {
            if (noMoneyUI == null) return;
            StartCoroutine(NoMoneyUITimer());
        }

        private IEnumerator NoMoneyUITimer()
        {
            if (_isCoroutineActive) yield break;
            _isCoroutineActive = true;
            
            noMoneyUI.SetActive(true);
            yield return new WaitForSeconds(uiTimer);
            noMoneyUI.SetActive(false);
            
            _isCoroutineActive = false;
        }
        #endregion

        #region CONFIRM_BUTTON_HANDLER
        public void InitializeIntList(UpgradeItemsList upgradeablesList)
        {
            foreach (var upgradeable in upgradeablesList.items)
            {
                itemLevelCounter.Add(0);
            }
        }

        public void SyncPagePosition(int currentPage)
        {
            _listPosition = currentPage;
        }

        private void UpdateLevelCounter(int addedLevel)
        {
            itemLevelCounter[_listPosition] += addedLevel;
            ValidateLevels();
        }

        private void ValidateLevels()
        {
            var validationCounter = 0;
            foreach (var itemLevel in itemLevelCounter)
            {
                if (itemLevel == 0)
                {

                    validationCounter += 1;
                }
            }

            if (validationCounter == itemLevelCounter.Count)
            {
                confirmButton.interactable = false;
                _noChanges = true;
            }
            else
            {
                confirmButton.interactable = true;
                _noChanges = false;
            }
        }

        public void ResetItemLevelCounter()
        {
            for (var index = 0; index < itemLevelCounter.Count; index++)
            {
                itemLevelCounter[index] = 0;
            }
            
            confirmButton.interactable = false;
            _noChanges = true;
        }
        #endregion
        
        private void OnEnable()
        {
            UpgradeShopEvents.OnInsufficientFunds += TriggerNoMoneyUI;
            UpgradeShopEvents.OnUpdateItemLevel += UpdateLevelCounter;
        }

        private void OnDisable()
        {
            UpgradeShopEvents.OnInsufficientFunds -= TriggerNoMoneyUI;
            UpgradeShopEvents.OnUpdateItemLevel -= UpdateLevelCounter;
        }
    }
}