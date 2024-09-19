using System.Collections.Generic;
using SaveSystem.Storage;
using ScriptableData;
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

    [SerializeField] private RectTransform[] _confiners;

    public GameObject ControlMenu;
    public Button rightSwitchButton;
    public Button leftSwitchButton;

    private List<RectTransform> buttonRects;
    private List<uiControls> buttonUIControls;
    private List<OnScreenButton> onScreenButtons;

    private bool isControllerMenuOpen;
    private GameStateSO gameStateSO;
    private bool isRightHudSwitched;
    private bool isLeftHudSwitched;

    private Vector3 posZero;
    private Vector3 posOne;
    private Vector3 posTwo;
    private Vector3 posThree;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        gameStateSO = GameManager.Instance.FetchGameStateData();
        controlfadePanel.alpha = 0;
    }

    private void Start()
    {
        InitializeUI();

        rightSwitchButton.onClick.AddListener(OnRightButtonClick);
        leftSwitchButton.onClick.AddListener(OnLeftButtonClick);
    }

    private void InitializeUI()
    {
        InitializeButtons();
        InitializeButtonPositions();
        InitializeConfinerPositions();

        if (RightPanel != null && LeftPanel != null)
        {
            RightPanel.enabled = false;
            LeftPanel.enabled = false;
        }

        ControlMenu.SetActive(false);
    }

    private void InitializeButtons()
    {
        buttonRects = new List<RectTransform>(buttonGOs.Count);
        buttonUIControls = new List<uiControls>(buttonGOs.Count);
        onScreenButtons = new List<OnScreenButton>(buttonGOs.Count);
        buttonSOs = new List<ButtonSO>(buttonGOs.Count);

        foreach (var buttonGO in buttonGOs)
        {
            if (buttonGO == null) continue;

            var rectTrans = buttonGO.GetComponent<RectTransform>();
            var uiControl = buttonGO.GetComponent<uiControls>();
            var onScreenButton = buttonGO.GetComponent<OnScreenButton>();

            if (rectTrans != null && uiControl != null && onScreenButton != null)
            {
                buttonRects.Add(rectTrans);
                buttonUIControls.Add(uiControl);
                onScreenButtons.Add(onScreenButton);
                buttonSOs.Add(uiControl.buttonSO);
            }
        }
    }

    private void InitializeButtonPositions()
    {
        for (int i = 0; i < buttonSOs.Count; i++)
        {
            InitializeButtonPosition(buttonSOs[i], buttonRects[i]);
        }
    }

    private void InitializeConfinerPositions()
    {
        if (_confiners.Length < 4) return;

        posZero = _confiners[0].localPosition;
        posOne = _confiners[1].localPosition;
        posTwo = _confiners[2].localPosition;
        posThree = _confiners[3].localPosition;
    }

    private void InitializeButtonPosition(ButtonSO buttonSO, RectTransform rectTrans)
    {
        if (buttonSO.inIntialPos && !gameStateSO.isPlayerFirstGame)
        {
            buttonSO.inIntialPos = false;
            buttonSO.CurrPos = rectTrans.position;
        }
    }

    private void ToggleControlScreen()
    {
        controlfadePanel.alpha = 1;
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

    public void ResetPositions()
    {
        for (int i = 0; i < buttonSOs.Count; i++)
        {
            buttonRects[i].localPosition = buttonSOs[i].InitPos;
            buttonSOs[i].inIntialPos = true;
        }

        if (_confiners.Length >= 4)
        {
            _confiners[0].localPosition = posZero;
            _confiners[1].localPosition = posOne;
            _confiners[2].localPosition = posTwo;
            _confiners[3].localPosition = posThree;
        }

        isRightHudSwitched = false;
        isLeftHudSwitched = false;
    }

    public void OnRightButtonClick()
    {
        if (!isRightHudSwitched)
        {
            _confiners[2].localPosition = posTwo;
            _confiners[3].localPosition = posThree;
        }
        else
        {
            _confiners[2].localPosition = posThree;
            _confiners[3].localPosition = posTwo;
        }
        isRightHudSwitched = !isRightHudSwitched;
    }

    public void OnLeftButtonClick()
    {
        if (!isLeftHudSwitched)
        {
            _confiners[0].localPosition = posZero;
            _confiners[1].localPosition = posOne;
        }
        else
        {
            _confiners[0].localPosition = posOne;
            _confiners[1].localPosition = posZero;
        }
        isLeftHudSwitched = !isLeftHudSwitched;
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

