using EventScripts;
using ScriptableData;
using UnityEngine;

namespace PowerUp.PowerUps
{
    public class Spring : PowerUpScript
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PowerUpsEvents.ACTIVATE_SPRING_PU?.Invoke();
                BaseEffect();
            }
        }
  
    }
}
