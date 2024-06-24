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
        public int scoreMultiplier;
        public bool springJump;
        public bool timeSlow;
        public bool expressDelivery;
        public bool singleBlockRemover;

        //tentative list
        [Header("Activated Upgrades")] 
        public int _barrierUpgrade = 1;
        public int _rezUpgrade = 0;
        
        [Header("Base upgrade stats")]
        public int _barrierDurability = 1;
        public float _barrierDuration = 2;
        public bool _canRez = false;
        

    }
}