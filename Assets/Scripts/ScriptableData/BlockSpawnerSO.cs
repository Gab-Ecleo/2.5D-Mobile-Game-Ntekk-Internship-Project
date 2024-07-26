using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Block Spawner Data", menuName = "Block Scriptables/Block Spawner Data", order = 0)]
    public class BlockSpawnerSO : ScriptableObject
    {
        public List<GameObject> mainBlockList;
        public List<GameObject> specialBlockList;
        public List<GameObject> powerUpBlockList;

        [Header("Block Type Spawn Rates")]
        [Tooltip("Value should be LOWER than heavy block & power up block")]
        [Range(1, 100)] public int defaultRate;
        [Tooltip("Value should be HIGHER than default block & LOWER than power up block. Set to 0 to keep heavy blocks from spawning")]
        [Range(0, 100)] public int heavyRate;
        [Tooltip("Value should be HIGHER than heavy block, unless heavy block is set to 0. Set to 0 to keep power up blocks from spawning")]
        [Range(0, 100)] public int powerUpRate;
    }
    
}