using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Game State Data", menuName = "Game State SO/Game State Data", order = 0)]
    public class GameStateSO : ScriptableObject
    {
        public bool isPaused;
        
        [Header("Tutorial")]
        public bool isPlayerFirstGame = true;

        [Header("Shop")]
        public bool isDefaultHomeButton = true;

        [Header("Player")]
        public Vector3 StartingPos;
        
        public void ResetPause()
        {
            isPaused = false;
        }
    }
}