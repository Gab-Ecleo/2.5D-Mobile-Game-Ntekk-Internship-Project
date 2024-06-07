using UnityEngine;

//This script turns the mobile controls on and off, depending on the platform
namespace ControlButtonScripts
{
    public class MobileControlSwitch : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(Application.isMobilePlatform);
        }
    }
}