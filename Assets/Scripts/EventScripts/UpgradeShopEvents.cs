using System;
using UnityEngine;

namespace EventScripts
{
    public class UpgradeShopEvents : MonoBehaviour
    {
        //Currency Events
        public static Action<float> OnPurchaseLevel;
        public static Action<float> OnSellLevel;
    }
}