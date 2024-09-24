using System;
using UnityEngine;
using UpgradeShop;

namespace EventScripts
{
    public class UpgradeShopEvents : MonoBehaviour
    {
        //Currency Events
        public static Action<int> OnPurchaseLevel;
        public static Action<int> OnSellLevel;

        public static Action OnUpdateCurrency;
        
        //UI Events
        public static Action OnInsufficientFunds;
        public static Action<int> OnUpdateItemLevel;
    }
}