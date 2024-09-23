using System;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

namespace SaveSystem.Storage
{
    [Serializable]
    public class ButtonData
    {
        public List<ButtonType> ButtonTypes = new List<ButtonType>();
        public List<Vector3> CurrPos = new List<Vector3>();

        public ButtonConfiners buttonConfiners = new ButtonConfiners();
    }
}