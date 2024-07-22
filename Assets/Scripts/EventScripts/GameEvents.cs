using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action<bool> IS_GAME_OVER;
    
    //HAZARDS
    public static Action TRIGGER_BLACKOUT_HAZARD;
    public static Action TRIGGER_RAIN_HAZARD;
    public static Action TRIGGER_ICE_HAZARD;
    public static Action TRIGGER_WIND_HAZARD;


    public static Action<int,int, bool> ON_SCORE_CHANGES;
    public static Action ON_UI_CHANGES;
    public static Action ON_PAUSE;
}
