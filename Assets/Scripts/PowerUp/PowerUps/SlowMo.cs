using UnityEngine;

namespace PowerUp.PowerUps
{
    public class SlowMo : PowerUpScript
    {
        /// <summary>
        /// Slowmo breaks player controller still need fix. RIOT PLS FIX
        /// </summary>
    
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatsSo.timeSlow = true;
                base.OnTriggerEnter(other);
            }
        }
    }
}
