using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class SliderPrefab
{
    public string StatsName;
    public Slider slider;
    public TMP_Text Number;
}

[System.Serializable]
public class TogglePrefab
{
    public string StatsName;
    public Toggle toggle;
}
