using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpEffects : ScriptableObject
{
    public abstract void ApplyEffect(GameObject player);
}
