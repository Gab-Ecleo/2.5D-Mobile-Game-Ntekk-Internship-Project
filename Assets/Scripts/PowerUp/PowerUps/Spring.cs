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
                PlayerStatsSo.stats.springJump = true;
                BaseEffect();
            }
            //Box Decay Trigger

        }
  
    }
}
