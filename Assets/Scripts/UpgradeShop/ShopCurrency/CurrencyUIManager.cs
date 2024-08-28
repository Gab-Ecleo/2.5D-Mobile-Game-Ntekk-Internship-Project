using TMPro;
using UnityEngine;

namespace UpgradeShop.ShopCurrency
{
    //handles the UI updates for the currency
    public class CurrencyUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyTxt;
        
        public void UpdateCurrencyUI(float updatedValue)
        {
            currencyTxt.text = updatedValue.ToString("D5");
        }
    }
}