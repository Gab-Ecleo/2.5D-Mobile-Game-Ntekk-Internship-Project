using System;
using System.Collections.Generic;
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
        [SerializeField] private GameStateSO gameStates;
        [SerializeField] private List<ButtonSO> buttonScriptableList;
        [SerializeField] private ButtonConfinerSO buttonConfinerStats;

        private void Awake()
        {
            InitializeFiles();
            if (SceneManager.GetActiveScene().name == "PlayableGameplay")
            {
                Debug.Log("SAVE DATA MANAGER ON PLAYABLE GAMEPLAY SCENE");
            }
        }

        private void InitializeFiles()
        {
            new UpgradeStorage().CreateUpgradeData();
            new StatStorages().CreateStatData();
            new CurrencyStorage().CreateCurrencyData();
            new AudioStorage().CreateAudioData();
            new ButtonStorage().CreateButtonData();
            new GameStateStorage().CreateGameStateData();
        }

        public void ClearData()
        {
            new UpgradeStorage().DeleteUpgradeData();
            new StatStorages().DeleteStatData();
            new CurrencyStorage().DeleteCurrencyData();
            new AudioStorage().DeleteAudioData();
            new ButtonStorage().DeleteButtonData();
            new GameStateStorage().DeleteGameStateData();
        }

        //called at the beginning of the scene
        private void InitializeData(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainMenu")
            {
                LoadUpgrade();
                LoadPlayerStats();
                LoadCurrencyData();
                LoadGameStateData();
                LoadAudioSettingsData();
                LoadButtons();
                UpgradeShopEvents.OnUpdateCurrency?.Invoke();
            }
        }

        #region PLAYER_STATS
        public void LoadPlayerStats()
        {
            if (new StatStorages().GetStatData() == null) return;
            initialPlayerStats.stats = new StatStorages().GetStatData().stats;
            Debug.Log("Loaded Player Stats");
        }

        public void SavePlayerStats()
        {
            var tempStats = new StatData() { stats = initialPlayerStats.stats};
            new StatStorages().SaveStatData(tempStats);
            Debug.Log("Saved Player Stats");
        }
        #endregion

        #region UPGRADES
        public void LoadUpgrade()
        {
            //called to update in-game data from the local storage
            if (new UpgradeStorage().GetUpgradeData() == null) return;
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
            Debug.Log("Loaded Upgrade Progress");
        }
        
        public void SaveUpgrade()
        {
            var tempUpgrades = new UpgradeData
            {
                items = itemList.items
            };
            new UpgradeStorage().SaveUpgradeData(tempUpgrades);
            
            Debug.Log("Saved Upgrade Progress");
        }
        #endregion

        #region CURRENCY
        public void LoadCurrencyData()
        {
            if (new CurrencyStorage().GetCurrencyData() == null) return;
            playerCurrency.coins = new CurrencyStorage().GetCurrencyData().coins;
            Debug.Log("Loaded Currency");
        }
        
        public void SaveCurrencyData()
        {
            var tempCurrency = new CurrencyData { coins = playerCurrency.coins};
            new CurrencyStorage().SaveCurrencyData(tempCurrency);
            Debug.Log("Saved Currency");
        }
        #endregion

        #region AUDIO_SETTINGS

        public void LoadAudioSettingsData()
        {
            if (new AudioStorage().GetAudioData() == null) return;
            audioSettings.bgmVolume = new AudioStorage().GetAudioData().bgmVolume;
            audioSettings.sfxVolume = new AudioStorage().GetAudioData().sfxVolume;
            Debug.Log("Loaded Audio Data");
        }

        public void SaveAudioSettingsData()
        {
            var tempData = new AudioData
            {
                bgmVolume = audioSettings.bgmVolume,
                sfxVolume = audioSettings.sfxVolume
            };
            new AudioStorage().SaveAudioData(tempData);
            Debug.Log("Saved Audio Data");
        }
        #endregion

        #region BUTTON_SETTINGS
        public void LoadButtons()
        {
            if (new ButtonStorage().GetButtonData() == null) return;

            foreach (var ScriptableData in buttonScriptableList)
            {
                var dataButtonList = new ButtonStorage().GetButtonData().ButtonTypes;
                for (var i = 0; i < dataButtonList.Count; i++)
                {
                    if (ScriptableData.ButtonType == dataButtonList[i])
                    {
                        ScriptableData.CurrPos = new ButtonStorage().GetButtonData().CurrPos[i];
                    }
                }
            }
            buttonConfinerStats.buttonConfiners = new ButtonStorage().GetButtonData().buttonConfiners;
            
            Debug.Log($"Loaded Button Data");
        }
        
        public void SaveButtons()
        {
            var tempData = new ButtonData();
            foreach (var scriptableData in buttonScriptableList)
            {
                tempData.ButtonTypes.Add(scriptableData.ButtonType);
                tempData.CurrPos.Add(scriptableData.CurrPos);
            }

            tempData.buttonConfiners = buttonConfinerStats.buttonConfiners;
            new ButtonStorage().SaveButtonData(tempData);
            Debug.Log("Saved Button Data");
        }
        #endregion

        #region GAME_STATES
        public void LoadGameStateData()
        {
            if (new GameStateStorage().GetGameStateData() == null) return;
            gameStates.isPaused = new GameStateStorage().GetGameStateData().isPaused;
            gameStates.isPlayerFirstGame = new GameStateStorage().GetGameStateData().isPlayerFirstGame;
            gameStates.isDefaultHomeButton = new GameStateStorage().GetGameStateData().isDefaultHomeButton;
            gameStates.StartingPos = new GameStateStorage().GetGameStateData().StartingPos;
            Debug.Log("Loaded Game State Data");
        }

        public void SaveGameStateData()
        {
            Debug.Log("Saving Game State Data");
            var tempdData = new GameStateData()
            {
                isPaused = gameStates.isPaused,
                isPlayerFirstGame = gameStates.isPlayerFirstGame,
                isDefaultHomeButton = gameStates.isDefaultHomeButton,
                StartingPos = gameStates.StartingPos
            };
            new GameStateStorage().SaveGameStateData(tempdData);
            Debug.Log("Saved Game State Data");
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

            LocalStorageEvents.OnLoadGameStateData += LoadGameStateData;
            LocalStorageEvents.OnSaveGameStateData += SaveGameStateData;
            
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
            
            LocalStorageEvents.OnLoadGameStateData -= LoadGameStateData;
            LocalStorageEvents.OnSaveGameStateData -= SaveGameStateData;
            
            SceneManager.sceneLoaded -= InitializeData;
        }
    }
}