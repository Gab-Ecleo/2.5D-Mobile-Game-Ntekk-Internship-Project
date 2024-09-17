using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Game State Data", menuName = "Game State SO/Game State Data", order = 0)]
    public class GameStateSO : ScriptableObject
    {
        public bool isPaused;
        
        public void ResetPause()
        {
            isPaused = false;
        }
    }
}