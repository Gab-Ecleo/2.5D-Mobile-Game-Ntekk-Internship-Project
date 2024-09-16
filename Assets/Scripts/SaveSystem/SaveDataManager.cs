using System;
using EventScripts;
using Player_Statistics;
using SaveSystem.Storage;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SaveSystem
{
    public class SaveDataManager : MonoBehaviour
    {
        private static SaveDataManager _instance;

        [SerializeField] private PlayerStatsSO initialPlayerStats;
        [SerializeField] private CurrencySO playerCurrency;
        [SerializeField] private UpgradeItemsList itemList;
        [SerializeField] private AudioSettingsSO audioSettings;
        
        private void Awake()
        {
            InitializeFiles();
        }

        private void InitializeFiles()
        {
            new UpgradeStorage().CreateUpgradeData();
            new StatStorages().CreateStatData();
            new CurrencyStorage().CreateCurrencyData();
            new AudioStorage().CreateAudioData();
            new ButtonStorage().CreateButtonData();
        }

        public void ClearData()
        {
            new UpgradeStorage().DeleteUpgradeData();
            new StatStorages().DeleteStatData();
            new CurrencyStorage().DeleteCurrencyData();
            new AudioStorage().DeleteAudioData();
            new ButtonStorage().DeleteButtonData();
        }

        //called at the beginning of the scene
        private void InitializeData(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 0)
            {
                LoadUpgrade();
                LoadPlayerStats();
                LoadCurrencyData();
                UpgradeShopEvents.OnUpdateCurrency?.Invoke();
            }
            
            //Can be called if there are no scripts manually loading these data
            if (scene.buildIndex != 0)
            {
                LoadCurrencyData();
            }
        }

        #region PLAYER_STATS
        public void LoadPlayerStats()
        {
            if (new StatStorages().GetStatData() == null) return;
            Debug.Log("Loading Player Stats");
            initialPlayerStats.stats = new StatStorages().GetStatData().stats;
        }

        public void SavePlayerStats()
        {
            Debug.Log("Saving Player Stats");
            var tempStats = new StatData() { stats = initialPlayerStats.stats};
            new StatStorages().SaveStatData(tempStats);
        }
        #endregion

        #region UPGRADES
        public void LoadUpgrade()
        {
            //called to update in-game data from the local storage
            if (new UpgradeStorage().GetUpgradeData() == null) return;
            Debug.Log("Loading Upgrade Progress");
            foreach (var dataItem in new UpgradeStorage().GetUpgradeData().items)
            {
                foreach (var item in itemList.items)
                {
                    if (dataItem.identifier == item.identifier)
                    {
                        item.currentLevel = dataItem.currentLevel;
                    }
                }
            }
        }
        
        public void SaveUpgrade()
        {
            Debug.Log("Saving Upgrade Progress");
            var tempUpgrades = new UpgradeData
            {
                items = itemList.items
            };
            new UpgradeStorage().SaveUpgradeData(tempUpgrades);
        }
        #endregion

        #region CURRENCY
        public void LoadCurrencyData()
        {
            if (new CurrencyStorage().GetCurrencyData() == null) return;
            Debug.Log("Loading Currency");
            playerCurrency.coins = new CurrencyStorage().GetCurrencyData().coins;
        }
        
        public void SaveCurrencyData()
        {
            Debug.Log("Saving Currency");
            var tempCurrency = new CurrencyData { coins = playerCurrency.coins};
            new CurrencyStorage().SaveCurrencyData(tempCurrency);
        }
        #endregion

        #region AUDIO_SETTINGS

        public void LoadAudioSettingsData()
        {
            if (new AudioStorage().GetAudioData() == null) return;
            Debug.Log("Loading Audio Data");
            audioSettings.bgmVolume = new AudioStorage().GetAudioData().bgmVolume;
            audioSettings.sfxVolume = new AudioStorage().GetAudioData().sfxVolume;
        }

        public void SaveAudioSettingsData()
        {
            Debug.Log("Saving Audio Data");
            var tempData = new AudioData
            {
                bgmVolume = audioSettings.bgmVolume,
                sfxVolume = audioSettings.sfxVolume
            };
            new AudioStorage().SaveAudioData(tempData);
        }
        #endregion

        #region BUTTON_SETTINGS
        public void LoadButtons()
        {
            //Add code if needed
        }
        
        public void SaveButtons()
        {
            //Saving progress is in UIControlManager
            //Add code if needed
        }
        #endregion

        private void OnEnable()
        {
            LocalStorageEvents.OnLoadPlayerStats += LoadPlayerStats;
            LocalStorageEvents.OnSavePlayerStats += SavePlayerStats;
            
            LocalStorageEvents.OnSaveUpgradesData += SaveUpgrade;
            LocalStorageEvents.OnLoadUpgradeData += LoadUpgrade;

            LocalStorageEvents.OnLoadCurrencyData += LoadCurrencyData;
            LocalStorageEvents.OnSaveCurrencyData += SaveCurrencyData;

            LocalStorageEvents.OnLoadAudioSettingsData += LoadAudioSettingsData;
            LocalStorageEvents.OnSaveAudioSettingsData += SaveAudioSettingsData;

            LocalStorageEvents.OnLoadButtonSettingsData += LoadButtons;
            LocalStorageEvents.OnSaveButtonSettingsData += SaveButtons;
            
            SceneManager.sceneLoaded += InitializeData;
        }

        private void OnDisable()
        {
            LocalStorageEvents.OnLoadPlayerStats -= LoadPlayerStats;
            LocalStorageEvents.OnSavePlayerStats -= SavePlayerStats;
            
            LocalStorageEvents.OnSaveUpgradesData -= SaveUpgrade;
            LocalStorageEvents.OnLoadUpgradeData -= LoadUpgrade;
            
            LocalStorageEvents.OnLoadCurrencyData -= LoadCurrencyData;
            LocalStorageEvents.OnSaveCurrencyData -= SaveCurrencyData;
            
            LocalStorageEvents.OnLoadAudioSettingsData -= LoadAudioSettingsData;
            LocalStorageEvents.OnSaveAudioSettingsData -= SaveAudioSettingsData;
            
            LocalStorageEvents.OnLoadButtonSettingsData -= LoadButtons;
            LocalStorageEvents.OnSaveButtonSettingsData -= SaveButtons;
            
            SceneManager.sceneLoaded -= InitializeData;
        }
    }
}