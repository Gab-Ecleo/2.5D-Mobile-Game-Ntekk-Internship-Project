using UnityEngine;

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
        public float jumpHeight = 8f;
        public float jumpFallOff = 0.7f;

        //tentative list
        [Header("Activated Power-ups")]
        public bool hasMultiplier;
        public bool springJump;
        public bool timeSlow;
        public bool expressDelivery;
        public bool singleBlockRemover;

        //tentative list
        [Header("Activated Upgrades")] 
        public int shield = 1;


    }
}