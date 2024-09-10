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
                PlayerStatsSo.stats.hasMultiplier = true;
                BaseEffect();
            }
        }
    }
}
