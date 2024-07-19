using TMPro;
using UnityEngine;

namespace UpgradeShop.ShopCurrency
{
    public class CurrencyUIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currencyTxt;
        
        public void UpdateCurrencyUI(float updatedValue)
        {
            currencyTxt.text = updatedValue.ToString();
        }
    }
}