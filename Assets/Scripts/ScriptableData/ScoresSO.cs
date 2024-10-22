using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu (fileName = "ScoreSystem", menuName = "Score")]
public class ScoresSO : ScriptableObject
{
    public int Points = 0;
    public int PointsToAdd = 2;

    public int Multiplier = 2;
    
    [Header("Additional Scores")]
    public int PlacementScore = 2;
    public int PUPickUpScore = 2;
}
