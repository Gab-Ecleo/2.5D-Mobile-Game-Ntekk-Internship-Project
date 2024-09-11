using EventScripts;
using UnityEngine;

namespace PowerUp.PowerUps
{
    public class SingleClear : PowerUpScript
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PowerUpsEvents.ACTIVATE_SINGLECLEAR_PU?.Invoke();
                BaseEffect();
            }
        }
    }
}
