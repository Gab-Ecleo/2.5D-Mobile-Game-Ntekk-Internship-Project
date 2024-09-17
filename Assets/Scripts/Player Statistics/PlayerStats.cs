using System;
using PlayerScripts;
using UnityEngine;

namespace Player_Statistics
{
    [Serializable]
    public class PlayerStats
    {
        [Header("Movement Stats")]
        public float movementSpeed = 5.5f;
        public float acceleration = 9f;
        public float deceleration = 9f;
        public float velPower = 1.2f;

        [Tooltip("A rate multiplier that reduces the player's movement speed on-air. Will only be calculated if the Movement State is set to 'Reduced Aerial Movement")]
        [Range(0f, 1f)] public float aerialSpdReducer = 0.0f;

        [Header("Jumping Stats")]
        public float jumpHeight = 8f;
        [Range(0, 1)]public float jumpCutMultiplier = 0.7f;

        [Header("Base upgrade stats")]
        public int barrierDurability = 0;
        public bool canRez = false;

        public Vector3 StartingPos;

        [Header("Tutorial")]
        public bool isPlayerFirstGame = true;

        [Header("Shop")]
        public bool isDefaultHomeButton = true;
    }
}