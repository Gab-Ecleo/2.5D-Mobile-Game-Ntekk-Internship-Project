using System;
using ScriptableData;
using Unity.VisualScripting;
using UnityEngine;

namespace PowerUp.PowerUps
{
    public class PowerUpScript : MonoBehaviour
    {
        protected void BaseEffect()
        {
            GameEvents.ON_SCORE_CHANGES(GameManager.Instance.FetchScores().PUPickUpScore);
            Destroy(gameObject);
        }
    }
}