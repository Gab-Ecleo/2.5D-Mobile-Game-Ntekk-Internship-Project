using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableData;
using UnityEngine.UI;
using EventScripts;

public class UIpowerUp : MonoBehaviour
{
    // UI Elements
    [SerializeField] private Image image;

    // Sprites Icon
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Sprite MultiplierIcon;
    [SerializeField] private Sprite SpringIcon;
    [SerializeField] private Sprite TimeSlowIcon;
    [SerializeField] private Sprite ExpressIcon;
    [SerializeField] private Sprite SingleIcon;

    private float initialDuration;

    private void OnEnable()
    {
        PowerUpsEvents.TRIGGER_POWERUPS_UI += PowerUpUIManger;
    }

    private void OnDisable()
    {
        PowerUpsEvents.TRIGGER_POWERUPS_UI -= PowerUpUIManger;
    }

    public void PowerUpUIManger(float currentDuration)
    {
        var powerUps = GameManager.Instance.FetchPowerUps();

        if (initialDuration == 0) 
        {
            initialDuration = currentDuration;
        }

        if (powerUps.hasMultiplier)
        {
            image.sprite = MultiplierIcon;
            UpdateDurationUI(currentDuration);
        }
        else if (powerUps.springJump)
        {
            image.sprite = SpringIcon;
            UpdateDurationUI(currentDuration);
        }
        else if (powerUps.timeSlow)
        {
            image.sprite = TimeSlowIcon;
            UpdateDurationUI(currentDuration);
        }
        else if (powerUps.expressDelivery)
        {
            image.sprite = ExpressIcon;
            UpdateDurationUI(currentDuration);
        }
        else if (powerUps.singleBlockRemover)
        {
            image.sprite = SingleIcon;
            UpdateDurationUI(currentDuration);
        }
        else
        {
            image.sprite = defaultIcon;
            ResetDuration();
        }
    }

    private void UpdateDurationUI(float currentDuration)
    {
        if (initialDuration > 0)
        {
            image.fillAmount = currentDuration / initialDuration; 
        }
    }

    public void ResetDuration()
    {
        initialDuration = 0;
        image.fillAmount = 1;
    }
}
