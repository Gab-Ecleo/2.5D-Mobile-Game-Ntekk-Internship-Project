using System;
using EventScripts;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace UpgradeShop.ShopCurrency
{
    //handles the updates given to the currency in the upgrade shop
    public class CurrencyManager : MonoBehaviour
    {
        [Header("In Match Modifiers")]
        [SerializeField] private int currencyDividend = 2;

        [Header("Data References")]
        [SerializeField] private CurrencySO currencyStats;
        [SerializeField] private ScoresSO scoresStats;
        
        [Header("Script References")]
        [SerializeField] private CurrencyUIManager UIManager;

        private void Start()
        {
            ResetMatchCoins();
        }

        #region SHOP RELATED
        private void InitializeCurrency()
        {
            UIManager.UpdateCurrencyUI(currencyStats.coins);
        }

        private void OnItemPurchase(int itemCost)
        {
            currencyStats.coins -= itemCost;
            UIManager.UpdateCurrencyUI(currencyStats.coins);
        }

        private void OnItemSell(int itemCost)
        {
            currencyStats.coins += itemCost;
            UIManager.UpdateCurrencyUI(currencyStats.coins);
        }
        #endregion

        #region INGAME RELATED
        private void UpdateInGameCurrency(int addedScore)
        {
            var addedCoins = addedScore / currencyDividend;
            currencyStats.matchCoins += addedCoins;
            currencyStats.coins += addedCoins;
            UIManager.UpdateCurrencyUI(currencyStats.matchCoins);
        }

        private void ResetMatchCoins()
        {
            currencyStats.matchCoins = 0;
        }
        #endregion

        private void OnEnable()
        {
            UpgradeShopEvents.OnPurchaseLevel += OnItemPurchase;
            UpgradeShopEvents.OnSellLevel += OnItemSell;

            UpgradeShopEvents.OnUpdateCurrency += InitializeCurrency;

            GameEvents.CONVERT_SCORE_TO_CURRENCY += UpdateInGameCurrency;
        }

        private void OnDisable()
        {
            UpgradeShopEvents.OnPurchaseLevel -= OnItemPurchase;
            UpgradeShopEvents.OnSellLevel -= OnItemSell;
            
            UpgradeShopEvents.OnUpdateCurrency -= InitializeCurrency;
            
            GameEvents.CONVERT_SCORE_TO_CURRENCY -= UpdateInGameCurrency;
        }
    }
}
