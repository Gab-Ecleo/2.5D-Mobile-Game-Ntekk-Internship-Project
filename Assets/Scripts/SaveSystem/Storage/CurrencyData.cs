using System;
using ScriptableData;
using UnityEngine;

namespace SaveSystem.Storage
{
    [Serializable]
    public class CurrencyData
    {
        [Header("Currencies")] 
        public int coins = 0;
    }
}