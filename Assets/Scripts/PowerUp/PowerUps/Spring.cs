using ScriptableData;
using UnityEngine;

namespace PowerUp.PowerUps
{
    public class Spring : PowerUpScript
    {
        protected override void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerStatsSo.springJump = true;
                base.OnTriggerEnter(other);
            }
            //Box Decay Trigger

        }
  
    }
}
