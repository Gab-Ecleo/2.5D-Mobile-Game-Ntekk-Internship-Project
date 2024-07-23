using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


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
        
        [Header("Item Info Text")]
        [SerializeField] private TextMeshProUGUI nextCostTxt;
        [SerializeField] private TextMeshProUGUI currentLvlStatTxt;
        [SerializeField] private TextMeshProUGUI nextLvlStatTxt;

        [Header("UI Buttons")]
        [SerializeField] private Button upgradeButton;
        [SerializeField] private Button degradeButton;

        public void UpdateDetailsUI(string nextCost, string currLvl, string nextLvl,StatSign statSign, bool maxLevel)
        {
            //set prefix and suffix here   
            #region PREFIX_&_SUFFIX
            //currency
            var nextCostWithCurrency = currencySign switch
            {
                CurrencySign.PHPeso => $"₱ {nextCost}",
                CurrencySign.JPYen => $"¥ {nextCost}",
                CurrencySign.SPPeseta => $"{nextCost} Pta",
                CurrencySign.USDollar => $"$ {nextCost}",
                _ => nextCostTxt.text
            };
            
            //stats
            var currLvlWithSign = statSign switch
            {
                StatSign.None => $"{currLvl}",
                StatSign.Percent => $"{currLvl}%",
                _ => throw new ArgumentOutOfRangeException(nameof(statSign), statSign, null)
            };
            var nextLvlWithSign = statSign switch
            {
                StatSign.None => $"{nextLvl}",
                StatSign.Percent => $"{nextLvl}%",
                _ => throw new ArgumentOutOfRangeException(nameof(statSign), statSign, null)
            };
            #endregion

            if (maxLevel)
            {
                nextCostTxt.text = $"Max Level";
                currentLvlStatTxt.text = currLvlWithSign;
                nextLvlStatTxt.text = $"Max Level";
                return;
            }

            nextCostTxt.text = nextCostWithCurrency;
            currentLvlStatTxt.text = currLvlWithSign;
            nextLvlStatTxt.text = nextLvlWithSign;
        }

        public void UpdateButtonsUI(int maxLevel, int currentLevel)
        {
            if (currentLevel == maxLevel)
            {
                upgradeButton.interactable = false;
            }
            else
            {
                if (!upgradeButton.interactable)
                {
                    upgradeButton.interactable = true;
                }
            }
            
            if (currentLevel == 0)
            {
                degradeButton.interactable = false;
            }
            else
            {
                if (!degradeButton.interactable)
                {
                    degradeButton.interactable = true;
                }
            }
        }
    }
}