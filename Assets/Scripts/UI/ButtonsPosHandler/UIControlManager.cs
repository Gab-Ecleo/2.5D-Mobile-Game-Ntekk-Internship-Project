using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.UI;

public class UIControlManager : MonoBehaviour
{
    private static UIControlManager _instance;
    public static UIControlManager Instance => _instance;

    [Header("Control Tween Animation")]
    [SerializeField] private GameObject controlPanelGO;
    [SerializeField] private CanvasGroup controlfadePanel;

    [Header("Movement Buttons")]
    private List<ButtonSO> buttonSOs;
    [SerializeField] private List<GameObject> buttonGOs;

    [Header("Panels")]
    [SerializeField] private Image RightPanel;
    [SerializeField] private Image LeftPanel;

    private List<RectTransform> buttonRects;
    private List<uiControls> buttonUIControls;
    private List<OnScreenButton> onScreenButtons;

    private bool isControllerMenuOpen = false;
    public GameObject ControlMenu;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitializeButtons();
    }

    private void InitializeButtons()
    {
        buttonRects = new List<RectTransform>();
        buttonUIControls = new List<uiControls>();
        onScreenButtons = new List<OnScreenButton>();
        buttonSOs = new List<ButtonSO>();

        for (int i = 0; i < buttonGOs.Count; i++)
        {
            if (buttonGOs[i] != null)
            {
                var rectTrans = buttonGOs[i].GetComponent<RectTransform>();
                var uiControl = buttonGOs[i].GetComponent<uiControls>();
                var onScreenButton = buttonGOs[i].GetComponent<OnScreenButton>();

                if (rectTrans != null && uiControl != null && onScreenButton != null)
                {
                    buttonRects.Add(rectTrans);
                    buttonUIControls.Add(uiControl);
                    onScreenButtons.Add(onScreenButton);

                    if (uiControl.buttonSO != null)
                    {
                        buttonSOs.Add(uiControl.buttonSO);
                    }
                    else
                    {
                        Debug.LogError($"Missing ButtonSO reference in uiControls for {buttonGOs[i].name}");
                    }
                }
                else
                {
                    Debug.LogError($"Missing components for button {buttonGOs[i].name}");
                }
            }
            else
            {
                Debug.LogError("Missing button GameObject reference");
            }
        }
    }

    private void Start()
    {
        if (RightPanel != null && LeftPanel != null)
        {
            RightPanel.enabled = false;
            LeftPanel.enabled = false;
        }
        ControlMenu.SetActive(false);

        InitializeButtonPositions();
    }

    private void InitializeButtonPositions()
    {
        for (int i = 0; i < buttonSOs.Count; i++)
        {
            InitializeButtonPosition(buttonSOs[i], buttonRects[i]);
        }
    }

    private void InitializeButtonPosition(ButtonSO buttonSO, RectTransform rectTrans)
    {
        if (buttonSO.inIntialPos)
        {
            buttonSO.inIntialPos = false;
            buttonSO.InitPos = rectTrans.position;
            buttonSO.CurrPos = rectTrans.position;
        }
        else
        {
            rectTrans.position = buttonSO.CurrPos;
        }
    }

    public void ResetPositions()
    {
        for (int i = 0; i < buttonSOs.Count; i++)
        {
            buttonRects[i].position = buttonSOs[i].InitPos;
        }
    }

    private void ToggleControlScreen()
    {
        isControllerMenuOpen = !isControllerMenuOpen;

        Time.timeScale = isControllerMenuOpen ? 0 : 1;
        ControlMenu.SetActive(isControllerMenuOpen);
        controlfadePanel.interactable = isControllerMenuOpen;
        controlfadePanel.blocksRaycasts = isControllerMenuOpen;

        RightPanel.enabled = isControllerMenuOpen;
        LeftPanel.enabled = isControllerMenuOpen;

        foreach (var control in buttonUIControls)
        {
            control.enabled = isControllerMenuOpen;
        }

        foreach (var onScreenButton in onScreenButtons)
        {
            onScreenButton.enabled = !isControllerMenuOpen;
        }
    }

    private void OnEnable()
    {
        GameEvents.ON_CONTROLS += ToggleControlScreen;
    }

    private void OnDisable()
    {
        GameEvents.ON_CONTROLS -= ToggleControlScreen;
    }
}
