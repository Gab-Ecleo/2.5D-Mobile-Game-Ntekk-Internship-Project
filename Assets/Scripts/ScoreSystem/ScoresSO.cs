using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu (fileName = "ScoreSystem", menuName = "Score")]
public class ScoresSO : ScriptableObject
{
    public int Points;
    public int PointsToAdd = 2;

    public int Multiplier = 1;
    public bool HasPowerUpMultiplier;
    public float PowerUpDuration = 3.0f;


    public static ScoresSO Instance;
}
