using System;
using ScriptableData;
using UnityEngine;

namespace SaveSystem.Storage
{
    [Serializable]
    public class StatData
    {
        [Header("Currencies")] 
        public float coins = 0f;
        
        [Header("Movement Stats")]
        public float movementSpeed = 8f;

        [Header("Aerial Movement Stats")]
        [Range(0.1f, 1f)] public float aerialSpdReducer = 0.8f;

        [Header("Jumping Stats")]
        public float jumpHeight = 8f;

        [Header("Base upgrade stats")]
        public int barrierDurability = 1;
        public bool canRez = false;
    }
}