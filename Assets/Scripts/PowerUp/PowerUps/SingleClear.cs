using UnityEngine;

namespace PowerUp.PowerUps
{
    public class SingleClear : PowerUpScript
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatsSo.stats.singleBlockRemover = true;
                BaseEffect();
            }
        }
    }
}
