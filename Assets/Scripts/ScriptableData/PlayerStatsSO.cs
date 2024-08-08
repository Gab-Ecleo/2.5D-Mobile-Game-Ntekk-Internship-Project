using System;
using Player_Statistics;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableData
{
    /// <summary>
    /// initial stats that will be fetch by local variables of player scripts. 
    /// </summary>
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "PlayerData", order = 0)]
    
    [Serializable]
    public class PlayerStatsSO : ScriptableObject
    {
        [SerializeField] public PlayerStats stats = new PlayerStats();
    }
}