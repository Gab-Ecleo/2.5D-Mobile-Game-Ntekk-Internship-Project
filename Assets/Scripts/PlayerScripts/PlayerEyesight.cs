using UnityEngine;

namespace PlayerScripts
{
    public class PlayerEyesight : MonoBehaviour
    {
        [Header("Raycast References")]
        [SerializeField] private LayerMask layerDetected;
        [SerializeField] private Vector3 rayOffset1, rayOffset2;
        [SerializeField] private float rayDistance = 1.05f;
        private RaycastHit _hit1, _hit2;
        private void FixedUpdate()
        {
            //Visually projects how the raycasts below would look like
            var position = transform.position;
            Debug.DrawRay(position + rayOffset1, transform.right * rayDistance, Color.blue);
            Debug.DrawRay(position + rayOffset2, transform.right * rayDistance, Color.blue);
        }
        
        //returns a gameObject if the raycast has detected the layer based on it. returns null if none
        public GameObject FirstBlockDetection()
        {
            Physics.Raycast(transform.position + rayOffset1, transform.right, out _hit1, rayDistance, layerDetected);
            if (_hit1.collider == null)
            {
                return null;
            }
            return _hit1.collider.gameObject;
        }
        
        //returns a gameObject if the raycast has detected the layer based on it. returns null if none
        public GameObject SecondBlockDetection()
        {
            Physics.Raycast(transform.position + rayOffset2, transform.right, out _hit2, rayDistance, layerDetected);
            if (_hit2.collider == null)
            {
                return null;
            }
            return _hit2.collider.gameObject;
        }
    }
}