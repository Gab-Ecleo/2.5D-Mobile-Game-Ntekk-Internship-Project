using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HazardManager : MonoBehaviour
{
    [SerializeField] private float hazardCooldown;
    [SerializeField] private List<string> hazardTag;

    private GameManager GAMEMANAGER;
    private bool _isCorActive;
    
    private void Start()
    {
        GAMEMANAGER = GameManager.Instance;
        
        if(GAMEMANAGER.IsGameOver()) return;

        //Initialize hazard after a delay
        Invoke(nameof(InitiateHazardSeq), hazardCooldown);
    }

    private void InitiateHazardSeq()
    {
        StartCoroutine(TriggerHazard());
    }

    IEnumerator TriggerHazard()
    {
        _isCorActive = true;
        
        if(GAMEMANAGER.IsGameOver()) yield break;
        
        //Trigger hazard base on the hazard Randomization.
        int hazardIndex = Random.Range(0, hazardTag.Count);
        string hazard = hazardTag[hazardIndex];
        
        Debug.Log("Loading Hazard Index of: " + hazardIndex);

        switch (hazard)
        {
            case "Rain":
                GameEvents.TRIGGER_RAIN_HAZARD?.Invoke();
                break;
            case "Blackout":
                GameEvents.TRIGGER_BLACKOUT_HAZARD?.Invoke();
                break;
            case "Ice":
                GameEvents.TRIGGER_ICE_HAZARD?.Invoke();
                break;
            case "Wind":
                GameEvents.TRIGGER_WIND_HAZARD?.Invoke();
                break;
        }

        hazardCooldown = Random.Range(10, 17);
        StartCoroutine(ShowWarning());
        
        yield return new WaitForSeconds(hazardCooldown);
        _isCorActive = false;
        
        InitiateHazardSeq();
    }

    IEnumerator ShowWarning()
    {
        float warningTimer = hazardCooldown - 3f;

        yield return new WaitForSeconds(warningTimer);
        //Show hazard warning
        Debug.LogWarning("Hazard Incoming");
    }
    
}
