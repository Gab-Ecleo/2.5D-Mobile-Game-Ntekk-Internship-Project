using System;
using SaveSystem.Storage;
using UnityEngine;

namespace SaveSystem
{
    public class SaveDataManager : MonoBehaviour
    {
        private void Awake()
        {
            InitializeFiles();
        }

        private void InitializeFiles()
        {
            new UpgradeStorage().CreateUpgradeData();
        }
    }
}