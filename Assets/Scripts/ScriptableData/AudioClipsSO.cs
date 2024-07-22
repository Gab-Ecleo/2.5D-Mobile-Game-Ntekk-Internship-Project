using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stores audio clips
[CreateAssetMenu(fileName = "Audio Clips Data", menuName = "SFX Clips", order = 0)]
public class AudioClipsSO : ScriptableObject
{
    [Header("Player SFX")]
    public AudioClip _jumpSFX;
    public AudioClip _pickupSFX;
    public AudioClip _dropSFX;
    public AudioClip _damageSFX;

    [Header("Hazard SFX")] 
    public AudioClip _rainSFX;
    public AudioClip _windSFX;
    public AudioClip _blackoutSFX;

    [Header("Background Audios")] 
    public AudioClip _firstLevelBGM;
    public AudioClip _secondLevelBGM;
    public AudioClip _thirdLevelBGM;
    public AudioClip _victoryBGM;
    public AudioClip _creditsBGM;
    public AudioClip _menuBGM;
    public AudioClip _deathBGM;
}
