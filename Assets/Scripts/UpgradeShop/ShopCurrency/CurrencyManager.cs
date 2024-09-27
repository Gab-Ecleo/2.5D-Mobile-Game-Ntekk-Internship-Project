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
        private static CurrencyManager _instance;
        public static CurrencyManager Instance => _instance;

        [Header("In Match Modifiers")]
        [SerializeField] private int currencyDividend = 2;

        [Header("Data References")]
        [SerializeField] private CurrencySO currencyStats;
        [SerializeField] private ScoresSO scoresStats;
        
        [Header("Script References")]
        [SerializeField] private CurrencyUIManager UIManager;
        private void Awake()
        {
            if (_instance == null) _instance = this;
            else Destroy(this);
        }

        private void Start()
        {
            ResetMatchCoins();

            InitializeCurrency();
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
            UIManager.UpdateMatchCurrencyUI(currencyStats.matchCoins);
        }

        public void ResetMatchCoins()
        {
            currencyStats.matchCoins = 0;
            UIManager.UpdateMatchCurrencyUI(currencyStats.matchCoins);
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
