using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "CurrencySo", menuName = "CurrencyData", order = 0)]
    public class CurrencySO : ScriptableObject
    {
        [Header("Currencies")] 
        public float coins = 0f;
        public float matchCoins = 0f;
    }
}