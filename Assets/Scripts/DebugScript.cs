using ScriptableData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DebugScript : MonoBehaviour
{
    [SerializeField] private PlayerStatsSO CurrentStats;

    [Header("Movement UI")]
    public TMP_Text MoveNumber;
    public Slider MovementSpeedSlider;

    [Header("Jump Height UI")]
    public TMP_Text JumpHeightNumber;
    public Slider JumpHeighSlider;

    [Header("Jump Fall UI")]
    public TMP_Text JumpFallNumber;
    public Slider JumpFallSlider;

    [Header("Multiplier Power-up UI")]
    public TMP_Text MultiplierText;
    public Toggle MultiplierBool;

    [Header("Spring Power-up UI")]
    public TMP_Text SpringText;
    public Toggle SpringBool;

    [Header("Time Slow Power-up UI")]
    public TMP_Text TimeSlowText;
    public Toggle TimeSlowBool;

    [Header("Barrier Upgrade UI")]
    public TMP_Text BarrierText;
    public Slider BarrierSlider;

    [Header("Rez Upgrade UI")]
    public TMP_Text RezText;
    public Slider RezSlider;

    private void Awake()
    {
        if(CurrentStats == null) { return; }
        if (MovementSpeedSlider == null) { return; }
    }

    // Start is called before the first frame update
    private void Start()
    {
        MovementSpeedSlider.value = CurrentStats.movementSpeed;
        JumpHeighSlider.value = CurrentStats.jumpHeight;
        JumpFallSlider.value = CurrentStats.jumpCutMultiplier;

        MultiplierBool.isOn = CurrentStats.hasMultiplier;
        SpringBool.isOn = CurrentStats.springJump;
        TimeSlowBool.isOn = CurrentStats.timeSlow;

        BarrierSlider.value = CurrentStats.barrierUpgrade;
        RezSlider.value = CurrentStats.rezUpgrade;
    }

    // Update is called once per frame
    private void Update()
    {
        MoveNumber.text = MovementSpeedSlider.value.ToString();
        JumpHeightNumber.text = JumpHeighSlider.value.ToString();
        JumpFallNumber.text = JumpFallSlider.value.ToString();

        MultiplierText.text = MultiplierBool.isOn.ToString();
        SpringText.text = SpringBool.isOn.ToString();
        TimeSlowText.text = TimeSlowBool.isOn.ToString();

        BarrierText.text = BarrierSlider.value.ToString();
        RezText.text = RezSlider.value.ToString();
    }

    public void UpdateMovementSpeed()
    {
        CurrentStats.movementSpeed = MovementSpeedSlider.value;
    }

    public void UpdateJumpHeight()
    {
        CurrentStats.jumpHeight = JumpHeighSlider.value;
    }
    public void UpdateJumpFall()
    {
        CurrentStats.jumpCutMultiplier = JumpFallSlider.value;
    }

    public void UpdateMultiplier()
    {
        CurrentStats.hasMultiplier = MultiplierBool.isOn;
    }

    public void UpdateSpring()
    {
        CurrentStats.springJump = SpringBool.isOn;
    }

    public void UpdateTimeSlow()
    {
        CurrentStats.timeSlow = TimeSlowBool.isOn;
    }
    public void UpdateBarrier()
    {
        CurrentStats.barrierUpgrade = ((int)BarrierSlider.value);
    }
    public void UpdateRez()
    {
        CurrentStats.rezUpgrade = ((int)RezSlider.value);
    }
}
