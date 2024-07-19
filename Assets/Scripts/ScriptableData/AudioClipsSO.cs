using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

//stores audio clips
[CreateAssetMenu(fileName = "Audio Clips Data", menuName = "SFX Clips", order = 0)]
public class AudioClipsSO : ScriptableObject
{
    [Header("Player SFX")]
    public AudioClip JumpSFX;
    public AudioClip PickupSFX;
    public AudioClip DropSFX;
    public AudioClip DamageSFX;
    public AudioClip FootstepSFX;

    [Header("Hazard SFX")] 
    public AudioClip RainSFX;
    public AudioClip WindSFX;
    public AudioClip BlackoutSFX;
}
