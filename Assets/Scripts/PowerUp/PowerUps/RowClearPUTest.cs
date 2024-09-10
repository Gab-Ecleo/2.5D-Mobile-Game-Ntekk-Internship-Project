using System;
using BlockSystemScripts;
using BlockSystemScripts.BlockScripts;
using ScriptableData;
using UnityEngine;

namespace PowerUp.PowerUps
{
    public class RowClearPUTest : PowerUpScript
    {
        [Header("Raycast References")]
        private BlockScript _blockScript;
        private RaycastHit _hit;

        private void Awake()
        {
            _blockScript = GetComponent<BlockScript>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatsSo.stats.expressDelivery = true;
                _blockScript.CurrentCell.AssignedRow.ClearRow();
                BaseEffect();
            }
        }
    }
}