using System.Collections;
using UnityEngine;
using ScriptableData;
using System.Reflection;
using BlockSystemScripts.BlockSpawnerScripts;
using UnityEngine.SceneManagement;
using EventScripts;
using Player_Statistics;
using UpgradeShop.ShopCurrency;
using System.Collections.Generic;


/// <summary>
/// NOTE:
/// ALL COMMENTED CODE CLUSTERS BELOW ARE SUBJECT TO CHANGE AS THE NEW POWER-UP SYSTEM HAS BEEN IMPLEMENTED ALREADY
/// </summary>
public class DebugManager : MonoBehaviour
{
    public SliderPrefab[] Sliders;

    [Header("Ref")]
    [SerializeField] private ScoresSO _playerScore;
    [SerializeField] private PlayerStatsSO _playerCurrStats;
    [SerializeField] private BlockSpawnersManager _blockSpawners;

    private Dictionary<string, float> _playerFloatStatsDict;
    private Dictionary<string, int> _playerIntStatsDict;
    private Dictionary<string, bool> _playerBoolStatsDict;

    private bool isDebugMenuOpen;
    [SerializeField] private GameObject DebugMenuScreen;
    //[SerializeField] private PlayerPowerUps _powerUps;

    private void Start()
    {
        isDebugMenuOpen = false;
        DebugMenuScreen.SetActive(false);

        InitializeDictionaries();
        InitializeStats();
    }

    public void ToggleDebug()
    {
        isDebugMenuOpen = !isDebugMenuOpen; // Simplified toggle
        DebugMenuScreen.SetActive(isDebugMenuOpen);
    }

    #region Stats
    private void InitializeDictionaries()
    {
        _playerFloatStatsDict = new Dictionary<string, float>();
        _playerIntStatsDict = new Dictionary<string, int>();
        _playerBoolStatsDict = new Dictionary<string, bool>();

        // Populate dictionaries from _playerCurrStats
        PopulateDictionary(_playerCurrStats.stats);
    }

    private void PopulateDictionary(PlayerStats stats)
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

        if (_playerFloatStatsDict == null && _playerIntStatsDict == null && _playerBoolStatsDict == null)
        {
            Debug.LogWarning("Empty stats");
            return;
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
    }

    public void UpdateUISliderStats(string Name)
    {
        SliderPrefab sliderPrefab = System.Array.Find(Sliders, slider => slider.StatsName == Name);

        if (sliderPrefab != null)
        {
            if (_playerFloatStatsDict.ContainsKey(Name))
            {
                _playerCurrStats.stats.GetType().GetField(Name)?.SetValue(_playerCurrStats.stats, sliderPrefab.slider.value);
                _playerFloatStatsDict[Name] = sliderPrefab.slider.value;
                sliderPrefab.Number.text = sliderPrefab.slider.value.ToString("0.0");
            }
            else if (_playerIntStatsDict.ContainsKey(Name))
            {
                _playerCurrStats.stats.GetType().GetField(Name)?.SetValue(_playerCurrStats.stats, (int)sliderPrefab.slider.value);
                _playerIntStatsDict[Name] = (int)sliderPrefab.slider.value;
                sliderPrefab.Number.text = ((int)sliderPrefab.slider.value).ToString();

                if (_playerIntStatsDict.ContainsKey("barrierDurability")) { PlayerEvents.ON_BARRIER_HIT?.Invoke(); }
            }
        }

    }

    #endregion

    #region buttons

    public void ResetPlayerPos()
    {
        PlayerEvents.OnPlayerPositionReset?.Invoke();
    }

    public void Currency(int addedPoints)
    {
        GameEvents.CONVERT_SCORE_TO_CURRENCY?.Invoke(addedPoints);
    }

    public void ResetCurrency()
    {
        CurrencyManager.Instance.ResetMatchCoins();
    }

    public void Scoring()
    {
        GameEvents.ON_SCORE_CHANGES?.Invoke(9);
    }

    public void SpawnBlock()
    {
        SpawnEvents.OnSpawnTrigger?.Invoke(false);
    }

    public void GameOver()
    {
        GameEvents.IS_GAME_OVER.Invoke(true);
    }

    #endregion
}
