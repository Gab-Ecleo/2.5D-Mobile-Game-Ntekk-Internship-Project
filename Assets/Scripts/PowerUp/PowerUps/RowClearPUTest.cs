using BlockSystemScripts.BlockScripts;
using ScriptableData;
using UnityEngine;

namespace PowerUp.PowerUps
{
    public class RowClearPUTest : PowerUpScript
    {
        [Header("Raycast References")]
        [SerializeField] private float rayDistance = 1.05f;
        [SerializeField] private LayerMask blockLayerDetected;
        private RaycastHit _hit;
        
        private void FixedUpdate() //For Testing. Can be Deleted
        {
            //draws a ray for the groundCheck raycast
            Debug.DrawRay(transform.position, -Vector3.up * rayDistance, Color.yellow);
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatsSo.expressDelivery = true;
                if (BottomRayDetection())
                {
                    BottomRayDetection().CurrentCell.AssignedRow.ClearRow();
                }
                base.OnTriggerEnter(other);
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