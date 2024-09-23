using System;
using UnityEngine;

namespace ScriptableData
{
    [CreateAssetMenu(fileName = "Button Confiner Data", menuName = "Button Scriptables/Button Confiner Data ", order = 0)]
    public class ButtonConfinerSO : ScriptableObject
    {
        public ButtonConfiners buttonConfiners = new ButtonConfiners();
    }

    [Serializable]
    public class ButtonConfiners
    {
        public bool isRightHudSwitched = false;
        public bool isLeftHudSwitched = false;

        [Header("Movement Button Confiners")]
        public Vector3 movementConfinerPos1;
        public Vector3 movementConfinerPos2;
        
        [Header("Action Button Confiners")]
        public Vector3 actionConfinerPos1;
        public Vector3 actionConfinerPos2;
    }
}