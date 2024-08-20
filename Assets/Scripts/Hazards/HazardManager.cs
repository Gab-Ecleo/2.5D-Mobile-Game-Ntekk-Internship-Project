using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class HazardManager : MonoBehaviour
{
    private static HazardManager _instance;
    public static HazardManager Instance { get { return _instance; } }

    [SerializeField] private float hazardCooldown;
    [SerializeField] private List<string> hazardTag;
    [SerializeField] private TextMeshProUGUI textWarning;
    [SerializeField] private TextMeshProUGUI currentHazard;

    private GameManager GAMEMANAGER;
    private bool _isCorActive;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GAMEMANAGER = GameManager.Instance;
        GAMEMANAGER.FetchHazardData().ResetAllData();
        
        if(GAMEMANAGER.IsGameOver()) return;

        //Initialize hazard after a delay
        StartCoroutine(ShowWarning());
        Invoke(nameof(InitiateHazardSeq), hazardCooldown);
    }

    private void InitiateHazardSeq()
    {
        StartCoroutine(TriggerHazard());
    }

    IEnumerator TriggerHazard()
    {
        _isCorActive = true;
        textWarning.gameObject.SetActive(false);
        
        if(GAMEMANAGER.IsGameOver()) yield break;
        
        //Trigger hazard base on the hazard Randomization.
        int hazardIndex = Random.Range(0, hazardTag.Count);
        string hazardType = hazardTag[hazardIndex];
        
        //Debug.Log($"Loading {hazardType} hazard");
        currentHazard.text = hazardType;

        switch (hazardType)
        {
            case "Rain":
                GameEvents.TRIGGER_RAIN_HAZARD?.Invoke();
                break;
            case "Blackout":
                GameEvents.TRIGGER_BLACKOUT_HAZARD?.Invoke();
                break;
            case "Wind":
                GameEvents.TRIGGER_WIND_HAZARD?.Invoke();
                break;
        }
        
        StartCoroutine(ShowWarning());
        
        yield return new WaitForSeconds(hazardCooldown);
        _isCorActive = false;
        Destroy(GameObject.FindWithTag("HazardFX"));
        
        InitiateHazardSeq();
    }

    IEnumerator ShowWarning()
    {
        float warningTimer = hazardCooldown - 3f;

        yield return new WaitForSeconds(warningTimer);
        //Show hazard warning
        Debug.LogWarning("Hazard Incoming");
        textWarning.gameObject.SetActive(true);
    }

    #region for Debugger
    public void TriggerHazard(string hazard)
    {
        StartCoroutine(TriggerHazardButton(hazard));
    }
    private IEnumerator TriggerHazardButton(string hazardTypes)
    {
        _isCorActive = true;
        textWarning.gameObject.SetActive(false);

        if (GAMEMANAGER.IsGameOver()) yield break;

        //Debug.Log($"Loading {hazardType} hazard");
        currentHazard.text = "Current Hazard: " + hazardTypes;

        switch (hazardTypes)
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
    #endregion
}
