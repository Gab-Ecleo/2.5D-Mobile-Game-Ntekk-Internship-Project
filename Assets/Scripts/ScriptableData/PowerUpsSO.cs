using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Power Ups Data", menuName = "Power Ups SO/Power Ups Data", order = 0)]
    public class PowerUpsSO : ScriptableObject
    {
        //tentative list
        [Header("Activated Power-ups")]
        public bool hasMultiplier = false;
        public bool springJump = false;
        public bool timeSlow = false;
        public bool expressDelivery = false;
        public bool singleBlockRemover = false;

        public void ResetValues()
        {
            hasMultiplier = false;
            springJump = false;
            timeSlow = false;
            expressDelivery = false;
            singleBlockRemover = false;
        }
    }
}