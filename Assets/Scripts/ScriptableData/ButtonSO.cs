using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ButtonSO", menuName = "ButtonSO")]
public class ButtonSO : ScriptableObject
{
    [SerializeField] private ButtonType buttonType;
    public ButtonType ButtonType => buttonType;

    private Vector3 _initPos = Vector3.zero;
    public Vector3 InitPos { get => _initPos; private set => _initPos = value;}

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