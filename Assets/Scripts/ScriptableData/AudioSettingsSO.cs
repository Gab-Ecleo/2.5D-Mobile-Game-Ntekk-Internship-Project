using UnityEngine;

namespace ScriptableData
{
    //stores the data of the whole game's audio settings
    [CreateAssetMenu(fileName = "Audio Settings Data", menuName = "Audio Settings/Audio Settings Data", order = 0)]
    public class AudioSettingsSO : ScriptableObject
    {
        public float bgmVolume;
        public float sfxVolume;
    }
}