using System;
using System.Collections;
using EventScripts;
using UnityEngine;

namespace UpgradeShop
{
    public class UpgradeShopUIManager : MonoBehaviour
    {
        [Header("Insufficient Funds UI")] 
        [SerializeField] private GameObject noMoneyUI;
        [SerializeField] private float uiTimer = 1f;

        private bool _isCoroutineActive;

        #region INSUFFICIENT_FUNDS
        private void TriggerNoMoneyUI()
        {
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
        
        private void OnEnable()
        {
            UpgradeShopEvents.OnInsufficientFunds += TriggerNoMoneyUI;
        }

        private void OnDisable()
        {
            UpgradeShopEvents.OnInsufficientFunds -= TriggerNoMoneyUI;
        }
    }
}