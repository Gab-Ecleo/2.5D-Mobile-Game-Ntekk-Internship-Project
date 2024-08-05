using System;
using UnityEngine;
using UpgradeShop;

namespace EventScripts
{
    public class UpgradeShopEvents : MonoBehaviour
    {
        //Currency Events
        public static Action<float> OnPurchaseLevel;
        public static Action<float> OnSellLevel;

        public static Action OnUpdateCurrency;
        
        //UI Events
        public static Action OnInsufficientFunds;
    }
}