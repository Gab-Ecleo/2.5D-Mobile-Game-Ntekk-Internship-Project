using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "CurrencySo", menuName = "CurrencyData", order = 0)]
    public class CurrencySO : ScriptableObject
    {
        [Header("Currencies")] 
        public int coins = 0;
        public int matchCoins = 0;
    }
}