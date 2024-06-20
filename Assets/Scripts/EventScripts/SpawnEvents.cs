using System;
using UnityEngine;

namespace EventScripts
{
    /// <summary>
    /// game events for spawning behavior
    /// </summary>
    public class SpawnEvents : MonoBehaviour
    {
        public static Action OnSpawnTrigger;
        public static Action OnSpawnTimerReset;
    }
}