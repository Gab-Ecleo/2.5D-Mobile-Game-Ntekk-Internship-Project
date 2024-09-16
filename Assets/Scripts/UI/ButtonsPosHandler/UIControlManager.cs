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

    [SerializeField] private List<ButtonSaveData> buttonSaveDataList;

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

    private bool isControllerMenuOpen = false;
    private PlayerStatsSO playerStatsSO;
    private bool isRightHudSwitched = false;
    private bool isLeftHudSwitched = false;

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
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        playerStatsSO = GameManager.Instance.FetchCurrentPlayerStat();
        controlfadePanel.alpha = 0;
    }

    private void InitializeButtons()
    {
        buttonRects = new List<RectTransform>();
        buttonUIControls = new List<uiControls>();
        onScreenButtons = new List<OnScreenButton>();
        buttonSOs = new List<ButtonSO>();

        foreach (var buttonGO in buttonGOs) 
        {
            if (buttonGO != null)
            {
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
    }

    private void Start()
    {
        if (buttonSOs == null || buttonRects == null || _confiners == null)
        {
            InitializeButtons();
        }

        LoadButtonData();

        if (RightPanel != null && LeftPanel != null)
        {
            RightPanel.enabled = false;
            LeftPanel.enabled = false;
        }

        ControlMenu.SetActive(false);

        InitializeButtonPositions();
        InitializeConfinerPositions();

        rightSwitchButton.onClick.AddListener(OnRightButtonClick);
        leftSwitchButton.onClick.AddListener(OnLeftButtonClick);
    }


    private void InitializeButtonPositions()
    {
        for (int i = 0; i < buttonSOs.Count; i++)
        {
            InitializeButtonPosition(buttonSOs[i], buttonRects[i]);
        }
    }

    public void InitializeConfinerPositions()
    {
        posZero = _confiners[0].localPosition;
        posOne = _confiners[1].localPosition;
        posTwo = _confiners[2].localPosition;
        posThree = _confiners[3].localPosition;
    }

    private void InitializeButtonPosition(ButtonSO buttonSO, RectTransform rectTrans)
    {
        if (buttonSO.inIntialPos && !playerStatsSO.stats.isPlayerFirstGame)
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

        _confiners[0].localPosition = posZero;
        _confiners[1].localPosition = posOne;
        _confiners[2].localPosition = posTwo;
        _confiners[3].localPosition = posThree;

        isRightHudSwitched = false;
        isLeftHudSwitched = false;
    }

    public void OnRightButtonClick()
    {
        if (!isRightHudSwitched)
        {
            _confiners[2].localPosition = posTwo;
            _confiners[3].localPosition = posThree;
            isRightHudSwitched = true;
        }
        else
        {
            _confiners[2].localPosition = posThree;
            _confiners[3].localPosition = posTwo;
            isRightHudSwitched = false;
        }
    }

    public void OnLeftButtonClick()
    {
        if (!isLeftHudSwitched)
        {
            _confiners[0].localPosition = posZero;
            _confiners[1].localPosition = posOne;
            isLeftHudSwitched = true;
        }
        else
        {

            _confiners[0].localPosition = posOne;
            _confiners[1].localPosition = posZero;
            
            isLeftHudSwitched = false;
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
    
    #region SAVING_AND_LOADING
    private void LoadButtonData()
    {
        if (new ButtonStorage().GetButtonData() == null) return;
        Debug.Log("Loading Button Data");
        
        //runs through each item from the json file
        foreach (var dataItem in new ButtonStorage().GetButtonData().items)
        {
            //runs through each item from the in-game list
            foreach (var item in buttonSaveDataList)
            {
                //if same button type, give the object the position data from the json file
                if (dataItem.buttonType == item.buttonType)
                {
                    item.Position = dataItem.Position;
                }
            }
        }
    }

    private void SaveButtonData()
    {
        buttonSaveDataList.Clear();

        foreach (var uiControl in buttonUIControls)
        {
            uiControl.SaveData();
            var saveData = new ButtonSaveData
            {
                Position = uiControl.buttonSO.CurrPos,
                buttonType = uiControl.buttonSO.ButtonType,
            };
            buttonSaveDataList.Add(saveData);
        }

        //Save data to Json
        var tempData = new ButtonData
        {
            items = buttonSaveDataList
        };
        new ButtonStorage().SaveButtonData(tempData);
        Debug.Log("Saving Button Data");
    }
    #endregion
    
    public void OnButtonsSave()
    {
        if (buttonSOs.Count == 4)
        {
            SaveButtonData();
        }
    }
}

[System.Serializable]
public class ButtonSaveData
{
    public Vector3 Position;
    public ButtonType buttonType;
}
