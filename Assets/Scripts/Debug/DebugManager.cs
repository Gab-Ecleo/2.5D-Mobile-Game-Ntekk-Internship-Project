using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using ScriptableData;
using System.Reflection;
using BlockSystemScripts.BlockSpawnerScripts;
using UnityEngine.SceneManagement;
using EventScripts;

[System.Serializable]
public class SliderPrefab
{
    public string StatsName;
    public Slider slider;
    public TMP_Text Number;
}

[System.Serializable]
public class ButtonPrefab
{
    public string FunctionName;
    public Button button;
}

[System.Serializable]
public class TogglePrefab
{
    public string StatsName;
    public Toggle toggle;
}

public class DebugManager : MonoBehaviour
{
    public SliderPrefab[] Sliders;
    public TogglePrefab[] Toggles;
    public ButtonPrefab[] Buttons;

    [Header("Ref")]
    [SerializeField] private ScoresSO _playerScore;
    [SerializeField] private PlayerStatsSO _playerCurrStats;
    [SerializeField] private BlockSpawnersManager _blockSpawners;

    private Dictionary<string, float> _playerFloatStatsDict;
    private Dictionary<string, int> _playerIntStatsDict;
    private Dictionary<string, bool> _playerBoolStatsDict;

    private void Start()
    {
        InitializeDictionaries();
        InitializeStats();
    }

    private void InitializeDictionaries()
    {
        _playerFloatStatsDict = new Dictionary<string, float>();
        _playerIntStatsDict = new Dictionary<string, int>();
        _playerBoolStatsDict = new Dictionary<string, bool>();


        // Populate dictionaries from _playerCurrStats
        PopulateDictionary(_playerCurrStats);
    }

    private void PopulateDictionary(PlayerStatsSO stats)
    {
        foreach (FieldInfo field in stats.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (field.FieldType == typeof(float))
            {
                _playerFloatStatsDict[field.Name] = (float)field.GetValue(stats);
            }
            else if (field.FieldType == typeof(int))
            {
                _playerIntStatsDict[field.Name] = (int)field.GetValue(stats);
            }
            else if (field.FieldType == typeof(bool))
            {
                _playerBoolStatsDict[field.Name] = (bool)field.GetValue(stats);
            }
        }
    }

    protected virtual void InitializeStats()
    {
        foreach (SliderPrefab sliderGO in Sliders)
        {
            if (sliderGO != null && _playerFloatStatsDict.ContainsKey(sliderGO.StatsName))
            {
                sliderGO.slider.value = _playerFloatStatsDict[sliderGO.StatsName];
                sliderGO.Number.text = _playerFloatStatsDict[sliderGO.StatsName].ToString("0.0");
            }
        }

        foreach (ButtonPrefab buttonGO in Buttons)
        {
            if (buttonGO.button != null)
            {
                buttonGO.button.onClick.AddListener(() => OnButtonClick(buttonGO.FunctionName));
            }
        }

        foreach (TogglePrefab toggleGO in Toggles)
        {
            if (toggleGO != null && _playerBoolStatsDict.ContainsKey(toggleGO.StatsName))
            {
                toggleGO.toggle.isOn = _playerBoolStatsDict[toggleGO.StatsName];
            }
        }
    }

    private void OnButtonClick(string functionName)
    {
        MethodInfo method = GetType().GetMethod(functionName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        if (method != null)
        {
            method.Invoke(this, null);
        }
        else
        {
            Debug.LogError($"Function {functionName} not found in {GetType().Name}");
        }
    }

    public void UpdateUISliderStats(string Name)
    {
        SliderPrefab sliderPrefab = System.Array.Find(Sliders, slider => slider.StatsName == Name);
        if (sliderPrefab != null && _playerFloatStatsDict.ContainsKey(Name))
        {
            _playerCurrStats.GetType().GetField(Name).SetValue(_playerCurrStats, sliderPrefab.slider.value);
            _playerFloatStatsDict[Name] = sliderPrefab.slider.value;
            sliderPrefab.Number.text = sliderPrefab.slider.value.ToString("0.0");
        }
        else if (sliderPrefab != null && _playerIntStatsDict.ContainsKey(Name))
        {
            _playerCurrStats.GetType().GetField(Name).SetValue(_playerCurrStats, sliderPrefab.slider.value);
            _playerIntStatsDict[Name] = (int)sliderPrefab.slider.value;
            sliderPrefab.Number.text = sliderPrefab.slider.value.ToString();
        }
        else
        {
            switch(Name)
            {
                case "SpawnRate":
                    Debug.Log("Spawn Rate Change");
                    break;
            }
        }

    }

    public void UpdateUIToggleStats(string Name)
    {
        TogglePrefab togglePrefab = System.Array.Find(Toggles, toggle => toggle.StatsName == Name);
        if (togglePrefab != null && _playerBoolStatsDict.ContainsKey(Name))
        {
            _playerCurrStats.GetType().GetField(Name).SetValue(_playerCurrStats, togglePrefab.toggle.isOn);
            _playerBoolStatsDict[Name] = togglePrefab.toggle.isOn;
        }
        else
        {
            switch (Name)
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
                case "BlockSpawn":
                    _blockSpawners.TriggerBlockSpawn();
                    break;
            }
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        GameEvents.IS_GAME_OVER?.Invoke(true);
    }

    public void ResetPlayerPos()
    {
        PlayerEvents.OnPlayerPositionReset?.Invoke();
    }

    public void ExpressDelivery()
    {
        // to be added
    }
}



