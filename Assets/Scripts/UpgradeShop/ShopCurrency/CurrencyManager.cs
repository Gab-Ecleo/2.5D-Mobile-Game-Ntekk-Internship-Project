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
        [SerializeField] private CurrencySO currencyStats;
        [SerializeField] private CurrencyUIManager UIManager;

        private void InitializeCurrency()
        {
            
            UIManager.UpdateCurrencyUI(currencyStats.coins);
        }

        private void OnItemPurchase(float itemCost)
        {
            currencyStats.coins -= itemCost;
            UIManager.UpdateCurrencyUI(currencyStats.coins);
        }

        private void OnItemSell(float itemCost)
        {
            currencyStats.coins += itemCost;
            UIManager.UpdateCurrencyUI(currencyStats.coins);
        }

        private void OnEnable()
        {
            UpgradeShopEvents.OnPurchaseLevel += OnItemPurchase;
            UpgradeShopEvents.OnSellLevel += OnItemSell;

            UpgradeShopEvents.OnUpdateCurrency += InitializeCurrency;
        }

        private void OnDisable()
        {
            UpgradeShopEvents.OnPurchaseLevel -= OnItemPurchase;
            UpgradeShopEvents.OnSellLevel -= OnItemSell;
            
            UpgradeShopEvents.OnUpdateCurrency -= InitializeCurrency;
        }
    }
}