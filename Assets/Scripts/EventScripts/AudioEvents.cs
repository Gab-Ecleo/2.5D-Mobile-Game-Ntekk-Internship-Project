using System;
using AudioScripts.AudioSettings;
using UnityEngine;
using AudioType = AudioScripts.AudioSettings.AudioType;

namespace EventScripts
{
    public class AudioEvents : MonoBehaviour
    {
        public static Action<object> OnValueChanged;
    }
}