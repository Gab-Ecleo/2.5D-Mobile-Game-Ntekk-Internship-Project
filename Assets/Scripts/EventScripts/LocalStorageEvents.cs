using System;
using UnityEngine;

namespace EventScripts
{
    public class LocalStorageEvents : MonoBehaviour
    {
        //Player Stats Events
        public static Action OnLoadPlayerStats;
        public static Action OnSavePlayerStats;
        
        //Upgrade Events
        public static Action OnLoadUpgradeData;
        public static Action OnSaveUpgradesData;
        
        //Currency Events
        public static Action OnLoadCurrencyData;
        public static Action OnSaveCurrencyData;
        
        //Audio Settings Events
        public static Action OnLoadAudioSettingsData;
        public static Action OnSaveAudioSettingsData;
        
        //Button Settings Events
        public static Action OnLoadButtonSettingsData;
        public static Action OnSaveButtonSettingsData;
    }
}