using System;
using EventScripts;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace UpgradeShop.ShopCurrency
{
    public class CurrencyManager : MonoBehaviour
    {
        [SerializeField] private PlayerStatsSO currencyStats;
        [SerializeField] private CurrencyUIManager UIManager;

        private void Start()
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
        }

        private void OnDisable()
        {
            UpgradeShopEvents.OnPurchaseLevel -= OnItemPurchase;
            UpgradeShopEvents.OnSellLevel -= OnItemSell;
        }
    }
}