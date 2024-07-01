﻿using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableData
{
    /// <summary>
    /// initial stats that will be fetch by local variables of player scripts. 
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerData", order = 0)]
    public class PlayerStatsSO : ScriptableObject
    {
        [Header("Movement Stats")] 
        public float movementSpeed = 8f;
        public float acceleration = 9f;
        public float decceleration = 9f;
        public float velPower = 1.2f;
        public float frictionAmount = 0.2f;

        [Header("Jumping Stats")]
        public float jumpHeight = 8f;
        [Range(0, 1)]public float jumpCutMultiplier = 0.7f;


        //tentative list
        [Header("Activated Power-ups")]
        public bool hasMultiplier;
        public bool springJump;
        public bool timeSlow;
        public bool expressDelivery;
        public bool singleBlockRemover;

        //tentative list
        [Header("Activated Upgrades")] 
        public int barrierUpgrade = 0;
        public int rezUpgrade = 0;
        
        [Header("Base upgrade stats")]
        public int barrierDurability = 1;
        public bool canRez = false;
        

    }
}