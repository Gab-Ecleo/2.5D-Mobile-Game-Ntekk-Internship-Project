using BlockSystemScripts;
using BlockSystemScripts.BlockScripts;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerEyesight : MonoBehaviour
    {
        [Header("Raycast for Block References")]
        [SerializeField] private LayerMask blockLayerDetected;
        [SerializeField] private LayerMask gridLayerDetected;
        [SerializeField] private Vector3 rayOffset1, rayOffset2;
        [SerializeField] private float rayDistance = 1.05f;
        private RaycastHit _hit1, _hit2, _hit3;
        
        private void FixedUpdate()
        {
            //Visually projects how the raycasts below would look like
            var position = transform.position;
            Debug.DrawRay(position + rayOffset1, transform.right * rayDistance, Color.blue);
            Debug.DrawRay(position + rayOffset2, transform.right * rayDistance, Color.blue);
        }
        
        //returns a BlockScript object if the raycast has detected the layer based on it. returns null if none
        public BlockScript FirstBlockDetection()
        {
            Physics.Raycast(transform.position + rayOffset1, transform.right, out _hit1, rayDistance, blockLayerDetected);
            if (_hit1.collider == null)
            {
                return null;
            }
            return _hit1.collider.gameObject.GetComponent<BlockScript>();
        }
        
        //returns a BlockScript object if the raycast has detected the layer based on it. returns null if none
        public BlockScript SecondBlockDetection()
        {
            Physics.Raycast(transform.position + rayOffset2, transform.right, out _hit2, rayDistance, blockLayerDetected);
            if (_hit2.collider == null)
            {
                return null;
            }
            return _hit2.collider.gameObject.GetComponent<BlockScript>();
        }
        
        //returns a GridCell object if the raycast has detected the layer based on it. returns null if none
        public GridCell GridCellDetection()
        {
            Physics.Raycast(transform.position + rayOffset1, transform.right, out _hit3, rayDistance, gridLayerDetected);
            if (_hit3.collider == null)
            {
                return null;
            }
            return _hit3.collider.gameObject.GetComponent<GridCell>();
        }
    }
}