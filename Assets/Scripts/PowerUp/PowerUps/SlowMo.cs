using UnityEngine;

namespace PowerUp.PowerUps
{
    public class SlowMo : PowerUpScript
    {
        /// <summary>
        /// Slowmo breaks player controller still need fix. RIOT PLS FIX
        /// </summary>
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatsSo.stats.timeSlow = true;
                BaseEffect();
            }
        }
    }
}
