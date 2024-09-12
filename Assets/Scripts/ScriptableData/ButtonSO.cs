using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ButtonSO", menuName = "ButtonSO")]
public class ButtonSO : ScriptableObject
{
    public ButtonType ButtonType;

    public Vector3 InitPos;
    public Vector3 CurrPos;

    public bool inIntialPos = true;
}

public enum ButtonType
{
    RightMoveButton,
    LeftMoveButton,
    JumpButton,
    InteractButton
}