using System;
using TMPro;
using UnityEngine;

namespace UpgradeShop
{
    public enum CurrencySign {
    PHPeso,
    JPYen,
    SPPeseta,
    USDollar,
    }

    public class ItemInfoUI : MonoBehaviour
    {
        [SerializeField] private CurrencySign currencySign; 
        
        [SerializeField] private TextMeshProUGUI nextCostTxt;
        [SerializeField] private TextMeshProUGUI currentLvlStatTxt;
        [SerializeField] private TextMeshProUGUI nextLvlStatTxt;

        public void UpdateDetailsUI(string nextCost, string currLvl, string nextLvl,StatSign statSign, bool maxLevel)
        {
            if (maxLevel)
            {
                nextCostTxt.text = $"Max Level";
                currentLvlStatTxt.text = statSign switch
                {
                    StatSign.None => $"{currLvl}",
                    StatSign.Percent => $"{currLvl}%",
                    _ => throw new ArgumentOutOfRangeException(nameof(statSign), statSign, null)
                };
                nextLvlStatTxt.text = $"Max Level";
                return;
            }
            
            nextCostTxt.text = currencySign switch
            {
                CurrencySign.PHPeso => $"₱ {nextCost}",
                CurrencySign.JPYen => $"¥ {nextCost}",
                CurrencySign.SPPeseta => $"Pta {nextCost}",
                CurrencySign.USDollar => $"$ {nextCost}",
                _ => nextCostTxt.text
            };
            
            currentLvlStatTxt.text = statSign switch
            {
                StatSign.None => $"{currLvl}",
                StatSign.Percent => $"{currLvl}%",
                _ => throw new ArgumentOutOfRangeException(nameof(statSign), statSign, null)
            };
            
            nextLvlStatTxt.text = statSign switch
            {
                StatSign.None => $"{nextLvl}",
                StatSign.Percent => $"{nextLvl}%",
                _ => throw new ArgumentOutOfRangeException(nameof(statSign), statSign, null)
            };
        }
    }
}