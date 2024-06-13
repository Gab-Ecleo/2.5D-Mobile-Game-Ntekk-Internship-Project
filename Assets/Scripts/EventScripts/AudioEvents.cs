using System;
using AudioScripts.AudioSettings;
using UnityEngine;
using AudioType = AudioScripts.AudioSettings.AudioType;

namespace EventScripts
{
    /// <summary>
    /// game events for audio behavior
    /// </summary>
    public class AudioEvents : MonoBehaviour
    {
        public static Action<object> OnValueChanged;
    }
}