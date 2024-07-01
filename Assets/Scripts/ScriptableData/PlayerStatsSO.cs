using UnityEngine;
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
        public float jumpHeight = 8f;
        public float jumpFallOff = 0.7f;

        //tentative list
        [Header("Activated Power-ups")]
        public bool hasMultiplier = false;
        public bool springJump = false;
        public bool timeSlow = false;
        public bool expressDelivery = false;
        public bool singleBlockRemover = false;

        //tentative list
        [Header("Activated Upgrades")] 
        public int barrierUpgrade = 0;
        public int rezUpgrade = 0;
        
        [Header("Base upgrade stats")]
        public int barrierDurability = 1;
        public bool canRez = false;
        

    }
}