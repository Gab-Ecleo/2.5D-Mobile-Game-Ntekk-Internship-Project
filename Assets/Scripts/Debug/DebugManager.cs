using System.Collections;
using UnityEngine;
using ScriptableData;
using System.Reflection;
using BlockSystemScripts.BlockSpawnerScripts;
using UnityEngine.SceneManagement;
using EventScripts;
using Player_Statistics;
using UpgradeShop.ShopCurrency;


/// <summary>
/// NOTE:
/// ALL COMMENTED CODE CLUSTERS BELOW ARE SUBJECT TO CHANGE AS THE NEW POWER-UP SYSTEM HAS BEEN IMPLEMENTED ALREADY
/// </summary>
public class DebugManager : MonoBehaviour
{
    public SliderPrefab[] Sliders;
    public TogglePrefab[] Toggles;

    [Header("Ref")]
    [SerializeField] private ScoresSO _playerScore;
    [SerializeField] private PlayerStatsSO _playerCurrStats;
    [SerializeField] private BlockSpawnersManager _blockSpawners;

    // private Dictionary<string, float> _playerFloatStatsDict;
    // private Dictionary<string, int> _playerIntStatsDict;
    // private Dictionary<string, bool> _playerBoolStatsDict;

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

    private void InitializeDictionaries()
    {
        // _playerFloatStatsDict = new Dictionary<string, float>();
        // _playerIntStatsDict = new Dictionary<string, int>();
        // _playerBoolStatsDict = new Dictionary<string, bool>();

        // Populate dictionaries from _playerCurrStats
        PopulateDictionary(_playerCurrStats.stats);
    }

    private void PopulateDictionary(PlayerStats stats)
    {
        // foreach (FieldInfo field in stats.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
        // {
        //     if (field.FieldType == typeof(float))
        //     {
        //         _playerFloatStatsDict[field.Name] = (float)field.GetValue(stats);
        //     }
        //     else if (field.FieldType == typeof(int))
        //     {
        //         _playerIntStatsDict[field.Name] = (int)field.GetValue(stats);
        //     }
        //     else if (field.FieldType == typeof(bool))
        //     {
        //         _playerBoolStatsDict[field.Name] = (bool)field.GetValue(stats);
        //     }
        // }
        //
        // if (_playerFloatStatsDict == null && _playerIntStatsDict == null && _playerBoolStatsDict == null)
        // {
        //     Debug.LogWarning("Empty stats");
        //      return;
        // }
    }

    protected virtual void InitializeStats()
    {
        // foreach (SliderPrefab sliderGO in Sliders)
        // {
        //     if (sliderGO != null && _playerFloatStatsDict.ContainsKey(sliderGO.StatsName))
        //     {
        //         sliderGO.slider.value = _playerFloatStatsDict[sliderGO.StatsName];
        //         sliderGO.Number.text = _playerFloatStatsDict[sliderGO.StatsName].ToString("0.0");
        //     }
        // }
        //
        // foreach (TogglePrefab toggleGO in Toggles)
        // {
        //     if (toggleGO != null && _playerBoolStatsDict.ContainsKey(toggleGO.StatsName))
        //     {
        //         toggleGO.toggle.isOn = _playerBoolStatsDict[toggleGO.StatsName];
        //     }
        // }
    }

    public void UpdateUISliderStats(string Name)
    {
        SliderPrefab sliderPrefab = System.Array.Find(Sliders, slider => slider.StatsName == Name);

        // if (sliderPrefab != null)
        // {
        //     if (_playerFloatStatsDict.ContainsKey(Name))
        //     {
        //         _playerCurrStats.stats.GetType().GetField(Name)?.SetValue(_playerCurrStats.stats, sliderPrefab.slider.value);
        //         _playerFloatStatsDict[Name] = sliderPrefab.slider.value;
        //         sliderPrefab.Number.text = sliderPrefab.slider.value.ToString("0.0");
        //     }
        //     else if (_playerIntStatsDict.ContainsKey(Name))
        //     {
        //         _playerCurrStats.stats.GetType().GetField(Name)?.SetValue(_playerCurrStats.stats, (int)sliderPrefab.slider.value);
        //         _playerIntStatsDict[Name] = (int)sliderPrefab.slider.value;
        //         sliderPrefab.Number.text = ((int)sliderPrefab.slider.value).ToString();
        //
        //         if (_playerIntStatsDict.ContainsKey("barrierDurability")) { PlayerEvents.ON_BARRIER_HIT?.Invoke(); }
        //     }
        //     else
        //     {
        //         Debug.LogError($"Field '{Name}' does not exist in the player's stats.");
        //     }
        // }
        // else
        // {
        //     Debug.LogError($"Slider with name '{Name}' not found.");
        // }
        
    }

    public void UpdateUIToggleStats(string Name)
    {
        //StartCoroutine(startPowerUp(Name));
    }

    // private IEnumerator startPowerUp(string Name)
    // {
    //     TogglePrefab togglePrefab = System.Array.Find(Toggles, toggle => toggle.StatsName == Name);
    //     if (togglePrefab != null && _playerBoolStatsDict.ContainsKey(Name))
    //     {
    //         _playerCurrStats.stats.GetType().GetField(Name).SetValue(_playerCurrStats.stats, togglePrefab.toggle.isOn);
    //         _playerBoolStatsDict[Name] = togglePrefab.toggle.isOn;
    //     } 
    //
    //     yield return new WaitForSeconds(1f);
    //
    //     _powerUps.PowerUpDebugging(true);
    //     togglePrefab.toggle.isOn = false;
    // }

    #region buttons
    public void LevelSelector(int GoToScene)
    {
        SceneManager.LoadScene(GoToScene);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {
        GameEvents.TRIGGER_GAMEEND_SCREEN?.Invoke(true);
    }

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

    public void ExpressDelivery()
    {
        // To be added
    }

    public void PauseMenu()
    {
        UIManager.Instance.TogglePauseButton();
    }

    public void Scoring()
    {
        int pointsToAdd = _playerScore.PointsToAdd;
        int multiplier = _playerScore.Multiplier;
        bool hasMultiplier = _playerCurrStats.stats.hasMultiplier;

        GameEvents.ON_SCORE_CHANGES?.Invoke(9, multiplier, hasMultiplier);
        GameEvents.ON_UI_CHANGES?.Invoke();
    }

    public void SpawnBlock()
    {
        SpawnEvents.OnSpawnTrigger?.Invoke(false);
    }

    public void HazardButton(string Hazard)
    {
        HazardManager.Instance.TriggerHazard(Hazard);
    }
    #endregion
}
