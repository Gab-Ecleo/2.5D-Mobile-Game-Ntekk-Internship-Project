using System;
using System.Collections;
using AudioScripts;
using AudioScripts.AudioSettings;
using UnityEngine;

public class BlackoutEffect : MonoBehaviour
{
    [SerializeField] private float hazardDuration = 5f;
    [SerializeField] private Animation anim;

    private GameManager _gameManager;
    private bool _isCorActive;
    private AudioClipsSO _audioClip;
    private AudioManager _audioManager;

    #region UNITY METHODS

    private void Awake()
    {
        GameEvents.TRIGGER_BLACKOUT_HAZARD += TriggerBoHazard;
    }

    private void OnDestroy()
    {
        GameEvents.TRIGGER_BLACKOUT_HAZARD -= TriggerBoHazard;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _isCorActive = false;
    }

    #endregion

    private void TriggerBoHazard()
    {
        if (_isCorActive || _gameManager.IsGameOver()) return;

        StartCoroutine(TriggerBlackout());
        // Plays SFX correlating to the action
        SfxScript.Instance.PlaySFXOneShot(_audioClip._blackoutSFX);
    }

    // Initiate a blackout which makes the screen go dark after [Hazard Duration].
    IEnumerator TriggerBlackout()
    {
        anim.Play("FadeIn");
        
        yield return new WaitForSeconds(hazardDuration);
        Debug.Log("Hazard Duration Ended");
        anim.Play("FadeOut");
    }
    
}
