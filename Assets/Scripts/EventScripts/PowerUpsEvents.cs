using System;
using UnityEngine;

namespace EventScripts
{
    public class PowerUpsEvents : MonoBehaviour
    {
        //POWER UPS
        public static Action ACTIVATE_SINGLECLEAR_PU;
        public static Action ACTIVATE_ROWCLEAR_PU;
        public static Action ACTIVATE_SPRING_PU;
        public static Action ACTIVATE_MULTIPLIER_PU;
        public static Action ACTIVATE_TIMESLOW_PU;
    
        public static Action DEACTIVATE_SINGLECLEAR_PU;
        public static Action DEACTIVATE_ROWCLEAR_PU;
        public static Action DEACTIVATE_SPRING_PU;
        public static Action DEACTIVATE_MULTIPLIER_PU;
        public static Action DEACTIVATE_TIMESLOW_PU;

        public static Action<float> TRIGGER_POWERUPS_UI;
    }
}