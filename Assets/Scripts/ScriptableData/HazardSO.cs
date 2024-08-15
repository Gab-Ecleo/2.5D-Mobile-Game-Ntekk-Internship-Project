using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Hazard Data", menuName = "Hazard Scriptables/Hazard Data", order = 0)]
    public class HazardSO : ScriptableObject
    {
        public bool IsRainActive;
        public bool IsBlackOutActive;
        public bool IsWindActive;

        public void ResetAllData()
        {
            IsRainActive = false;
            IsBlackOutActive = false;
            IsWindActive = false;
        }
    }
}