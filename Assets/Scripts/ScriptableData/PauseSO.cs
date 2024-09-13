using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Pause Data", menuName = "Pause SO/Pause Data", order = 0)]
    public class PauseSO : ScriptableObject
    {
        public bool isPaused;

        public void ResetPause()
        {
            isPaused = false;
        }
    }
    
    
}