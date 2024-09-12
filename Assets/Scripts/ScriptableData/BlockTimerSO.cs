using System;
using UnityEngine;

namespace ScriptableData
{
    public enum BlockTimerState
    {
        Normal,
        Slowed
    }

    [CreateAssetMenu(fileName = "BlockTimerData", menuName = "Block Scriptables/BlockTimer Data", order = 0)]
    public class BlockTimerSO : ScriptableObject
    {
        public BlockTimerState blockTimerState = BlockTimerState.Normal;
        public float initialTimer = 0.5f;
        public float slowedTimer = 1f;

        public void ResetState()
        {
            blockTimerState = BlockTimerState.Normal;
        }
    }
}