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

    // pause menu, tutorial, and score events
    public static Action<int,int, bool> ON_SCORE_CHANGES;
    public static Action ON_UI_CHANGES;
    public static Action ON_PAUSE;
    public static Action ON_TUTORIAL_UNLOCKED;
    public static Action ON_TUTORIAL;

    //GAME END UI
    public static Action TRIGGER_GAMEEND_SCREEN;
    
    //IN-GAME CURRENCY EVENTS
    public static Action<int> CONVERT_SCORE_TO_CURRENCY;
}
