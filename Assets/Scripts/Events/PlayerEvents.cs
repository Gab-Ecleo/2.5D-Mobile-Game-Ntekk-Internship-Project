using System;
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
        public static Action OnPlayerDamage;
    }
}