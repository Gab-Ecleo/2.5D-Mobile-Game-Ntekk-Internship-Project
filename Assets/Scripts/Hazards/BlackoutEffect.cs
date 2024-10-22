using System;
using System.Collections;
using AudioScripts;
using AudioScripts.AudioSettings;
using EventScripts;
using UnityEngine;

public class BlackoutEffect : MonoBehaviour
{
    [SerializeField] private float hazardDuration = 5f;
    [SerializeField] private Animation blackOutAnimation;
    [SerializeField] private Animation whiteOutAnimation;
    [SerializeField] public GameObject _blackoutParticles;

    private GameManager _gameManager;
    private bool _isCorActive;
    
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

    // public for debugging
    public void TriggerBoHazard()
    {
        if (_isCorActive || _gameManager.IsGameOver()) return;

        StartCoroutine(TriggerBlackout());
        // Plays SFX correlating to the action
    }
    
    // Initiate a blackout which makes the screen go dark after [Hazard Duration].
    IEnumerator TriggerBlackout()
    {
        _gameManager.FetchHazardData().IsBlackOutActive = true;
        
        AudioEvents.ON_HAZARD_TRIGGER?.Invoke("blackout");
        
        blackOutAnimation.Play("FadeIn");
        var blackoutParticles = Instantiate(_blackoutParticles);
        whiteOutAnimation.Play("Flicker");
        yield return new WaitForSeconds(hazardDuration / 2);
        
        whiteOutAnimation.Play("Flicker");
        
        yield return new WaitForSeconds(hazardDuration / 2);
        
        Debug.Log("Hazard Duration Ended");
        SfxScript.Instance.StopSFX();
        _blackoutParticles.SetActive(false);
        blackOutAnimation.Play("FadeOut");
        _gameManager.FetchHazardData().IsBlackOutActive = false;
    }
}
