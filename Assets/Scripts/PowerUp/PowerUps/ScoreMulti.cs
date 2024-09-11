using EventScripts;
using ScriptableData;
using UnityEngine;

namespace PowerUp.PowerUps
{
    public class ScoreMulti : PowerUpScript
    {
        // Update is called once per frame
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PowerUpsEvents.ACTIVATE_MULTIPLIER_PU?.Invoke();
                BaseEffect();
            }
        }
    }
}
