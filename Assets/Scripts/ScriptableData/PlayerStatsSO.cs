using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableData
{
    public enum PlayerMovementState
    {
        WithAerialMovement,
        ReducedFlippedMovement,
        ReducedAerialMovement,
        NoAerialMovement,
    }
    
    /// <summary>
    /// initial stats that will be fetch by local variables of player scripts. 
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerData", order = 0)]
    
    public class PlayerStatsSO : ScriptableObject
    {
        [Header("Currencies")] 
        public float coins = 0f;
        
        [Header("Movement Stats")]
        public bool canPlayerMove;
        public float movementSpeed = 8f;
        public float acceleration = 9f;
        public float deceleration = 9f;
        public float velPower = 1.2f;
        public float frictionAmount = 0.2f;
        
        [Header("Movement Implementation Choice")]
        public PlayerMovementState movementState;
        [Tooltip("A rate multiplier that reduces the player's movement speed on-air. Will only be calculated if the Movement State is set to 'Reduced Aerial Movement")]
        [Range(0.1f, 1f)] public float aerialSpdReducer = 0.8f;

        [Header("Jumping Stats")]
        public float jumpHeight = 8f;
        [Range(0, 1)]public float jumpCutMultiplier = 0.7f;
        
        //tentative list
        [Header("Activated Power-ups")]
        public bool hasMultiplier = false;
        public bool springJump = false;
        public bool timeSlow = false;
        public bool expressDelivery = false;
        public bool singleBlockRemover = false;

        //tentative list
        [Header("Activated Upgrades")]
        public bool isBarrierUpgraded;
        public bool isRezUpgraded;

        public int barrierUpgrade = 0;
        public int rezUpgrade = 0;
        
        [Header("Base upgrade stats")]
        public int barrierDurability = 1;
        public bool canRez = false;

        public Vector3 StartingPos;
    }
}