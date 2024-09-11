using System;
using BlockSystemScripts;
using BlockSystemScripts.BlockScripts;
using EventScripts;
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
                PowerUpsEvents.ACTIVATE_ROWCLEAR_PU?.Invoke();
                _blockScript.CurrentCell.AssignedRow.ClearRow();
                PowerUpsEvents.DEACTIVATE_ROWCLEAR_PU?.Invoke();
                BaseEffect();
            }
        }
    }
}