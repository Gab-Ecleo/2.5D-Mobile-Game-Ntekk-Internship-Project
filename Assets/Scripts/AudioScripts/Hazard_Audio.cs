using AudioScripts;
using AudioScripts.AudioSettings;
using EventScripts;
using UnityEngine;

public class Hazard_Audio : MonoBehaviour
{
    private AudioClipsSO _audioClips;
    private SfxScript _src;

    private void Awake()
    {
        AudioEvents.ON_HAZARD_TRIGGER += PlayHazardAudio;
    }

    private void Start()
    {
        _src = SfxScript.Instance;
        _audioClips = AudioManager.Instance.FetchAudioClips();
    }

    private void OnDestroy()
    {
        AudioEvents.ON_HAZARD_TRIGGER -= PlayHazardAudio;
    }

    private void PlayHazardAudio(string audioStr)
    {
        switch (audioStr)
        {
            case "blackout":
                _src.PlaySFXOneShot(_audioClips.BlackoutSFX);
                break;
            
            case "wind":
                _src.PlaySFXOneShot(_audioClips.WindSFX);
                break;
            
            case "rain":
                _src.PlaySFXOneShot(_audioClips.RainSFX);
                break;
        }
    }
}
