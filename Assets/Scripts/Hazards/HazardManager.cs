using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class HazardManager : MonoBehaviour
{
    private static HazardManager _instance;
    public static HazardManager Instance { get { return _instance; } }

    [SerializeField] private float hazardCooldown;
    [SerializeField] private List<string> hazardTag;
    [SerializeField] private Image textWarning;
    [SerializeField] private Image currentHazard;

    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite rainSprite;
    [SerializeField] private Sprite blackoutSprite;
    [SerializeField] private Sprite windSprite;

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

        switch (hazardType)
        {
            case "Rain":
                GameEvents.TRIGGER_RAIN_HAZARD?.Invoke();
                currentHazard.sprite = rainSprite;
                break;
            case "Blackout":
                GameEvents.TRIGGER_BLACKOUT_HAZARD?.Invoke();
                currentHazard.sprite = blackoutSprite;
                break;
            case "Wind":
                GameEvents.TRIGGER_WIND_HAZARD?.Invoke();
                currentHazard.sprite = windSprite;
                break;
        }
        
        StartCoroutine(ShowWarning());
        
        yield return new WaitForSeconds(hazardCooldown);
        currentHazard.sprite = defaultSprite;
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


}
