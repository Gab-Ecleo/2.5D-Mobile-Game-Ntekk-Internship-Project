using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action<bool> IS_GAME_OVER;

    public static Action<int,int, bool> ON_SCORE_CHANGES;
    public static Action ON_UI_CHANGES;
}
