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
        
        //UI Events
        public static Action OnInsufficientFunds;
    }
}