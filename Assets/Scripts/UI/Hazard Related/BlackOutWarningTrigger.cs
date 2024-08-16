using UnityEngine;

namespace UI.Hazard_Related
{
    public class BlackOutWarningTrigger : MonoBehaviour
    {
        public void DisableObject()
        {
            gameObject.SetActive(false);
        }
    }
}