using System;
using BlockSystemScripts.BlockScripts;
using ScriptableData;
using UnityEngine;

namespace PowerUp
{
    public class RowClearPUTest : MonoBehaviour
    {
        [SerializeField] private PlayerStatsSO _playerStatsSo;
        private PlayerPowerUps _powerUp;
        
        [Header("Raycast References")]
        [SerializeField] private float rayDistance = 1.05f;
        [SerializeField] private LayerMask blockLayerDetected;
        private RaycastHit _hit;

        private void Start()
        {
            _playerStatsSo = Resources.Load("PlayerData/CurrentPlayerStats") as PlayerStatsSO;
            _powerUp = GameObject.FindWithTag("Player").GetComponent<PlayerPowerUps>();
        }
        
        private void FixedUpdate() //For Testing. Can be Deleted
        {
            //draws a ray for the groundCheck raycast
            Debug.DrawRay(transform.position, -Vector3.up * rayDistance, Color.yellow);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _playerStatsSo.expressDelivery = true;
                
                _powerUp.PowerUp();
                if (BottomRayDetection())
                {
                    BottomRayDetection().CurrentCell.AssignedRow.ClearRow();
                }
                
                Destroy(gameObject);
            }
        }

        private BlockScript BottomRayDetection()
        {
            Physics.Raycast(transform.position, -Vector3.up, out _hit, rayDistance, blockLayerDetected);
            if (_hit.collider == null)
            {
                return null;
            }
            return _hit.collider.gameObject.GetComponent<BlockScript>();
        }
    }
}