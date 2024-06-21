using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    [SerializeField] private float hazardCooldown;
    [SerializeField] private List<string> hazardTag;

    private GameManager GAMEMANAGER;
    
    private void Start()
    {
        GAMEMANAGER = GameManager.Instance;
        
        if(GAMEMANAGER.IsGameOver()) return;

        //Initialize hazard after a delay
        Invoke(nameof(TriggerHazard), hazardCooldown);
    }

    IEnumerator TriggerHazard()
    {
        if(GAMEMANAGER.IsGameOver()) yield break;

        Debug.Log("Loading Hazard");
        
        //Trigger hazard base on the hazard Randomization.
        int hazardIndex = Random.Range(1, hazardTag.Count + 1);
        string hazard = hazardTag[hazardIndex];

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
        
        yield return new WaitForSeconds(hazardCooldown);
        TriggerHazard();
    }
}
