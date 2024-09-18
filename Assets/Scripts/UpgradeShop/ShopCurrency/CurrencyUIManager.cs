using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace UpgradeShop.ShopCurrency
{
    //handles the UI updates for the currency
    public class CurrencyUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyTxt;
        [SerializeField] private TextMeshProUGUI matchCurrencyTxt;
        
        //called for the shop currency
        public void UpdateCurrencyUI(int updatedValue)
        {
            // no value = no UI to update
            if (currencyTxt == null) return;
            currencyTxt.text = updatedValue.ToString("D4");
        }

        //called for the in-match currency
        public void UpdateMatchCurrencyUI(int updatedValue)
        {
            // no value = no UI to update
            if (matchCurrencyTxt == null) return;
            matchCurrencyTxt.text = updatedValue.ToString("D4");
        }
    }
}