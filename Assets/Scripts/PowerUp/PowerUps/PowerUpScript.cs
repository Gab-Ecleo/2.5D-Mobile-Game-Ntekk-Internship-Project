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
            Destroy(gameObject);
        }
    }
}