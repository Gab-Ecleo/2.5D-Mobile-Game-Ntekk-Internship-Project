using UnityEngine;

namespace PowerUp.PowerUps
{
    public class SingleClear : PowerUpScript
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatsSo.stats.singleBlockRemover = true;
                base.OnTriggerEnter(other);
            }
        }
    }
}
