using System;
using ScriptableData;
using UnityEngine;

namespace EventScripts
{
    /// <summary>
    /// game events for player behavior
    /// </summary>
    public class PlayerEvents : MonoBehaviour
    {
        public static Action OnPlayerDamage;

        public static Action OnPlayerPositionReset;

        public static Action ON_BARRIER_HIT;
        
        //Movement
        public static Action ON_PLAYER_JUMP;
        public static Action ON_PLAYER_PICKUP;
        public static Action ON_PLAYER_DROP;
        public static Action<bool> PLAYER_ISGROUNDED;
    }
}