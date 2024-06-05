using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerGrabThrow : MonoBehaviour
    {
        public void Interact(InputAction.CallbackContext ctx)
        {
            Debug.Log("Player has Grabbed");
        }

        private void Throw()
        {
            Debug.Log("Player has Thrown");
        }
    }
}