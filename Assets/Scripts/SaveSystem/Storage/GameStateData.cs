using System;
using Vector3 = UnityEngine.Vector3;

namespace SaveSystem.Storage
{
    [Serializable]
    public class GameStateData
    {
        public bool isPaused;
        public bool isPlayerFirstGame = true;
        public bool isDefaultHomeButton = true;
        public Vector3 StartingPos;
    }
}