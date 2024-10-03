using System;
using UnityEngine;

namespace EventScripts
{
    /// <summary>
    /// game events for audio behavior
    /// </summary>
    public class AudioEvents : MonoBehaviour
    {
        public static Action<object> OnValueChanged;
        public static Action ON_STOP_SFX;
        public static Action ON_STOP_BGM;
        public static Action ON_GAME_START;

        public static Action ON_PLAYER_JUMP;
        public static Action ON_PLAYER_PICKUP;
        public static Action ON_PLAYER_DROP;
        public static Action ON_PLAYER_HIT;
        public static Action ON_PLAYER_DEATH;

        public static Action<string> ON_HAZARD_TRIGGER;
        public static Action<int> RANDOMIZE_AUDIO;
    }
}