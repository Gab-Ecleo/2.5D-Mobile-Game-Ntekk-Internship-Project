using UnityEngine;

namespace Events
{
    /// <summary>
    /// game events for player behavior
    /// 
    /// TRANSFER THIS SCRIPT TO A "GAME EVENT" NAMESPACE
    /// </summary>
    public class PlayerEvents : MonoBehaviour
    {
        public delegate void PlayerDamage();
        public static event PlayerDamage OnPlayerDamage;
        public static void OnPlayerDamageMethod()
        {
            if (OnPlayerDamage != null)
            {
                OnPlayerDamage();
            }
        }
    }
}