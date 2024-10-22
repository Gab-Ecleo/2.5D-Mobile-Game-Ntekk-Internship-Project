using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using ScriptableData;

public class TabGroup : MonoBehaviour
{
    private static TabGroup _instance;
    public static TabGroup Instance => _instance;

    public List<TabButton> tabButtons;
    public List<GameObject> objectsToSwap;
    [Header("Sprite")]


    [Header("Tabs")]
    private TabButton _selectedTab;
    public bool tabIsOpen;
    public ButtonTimerState timerState = ButtonTimerState.Ready;
    public float cooldownTime = 1f;

    public int index;

    [Header("Robot Animation")]
    public GameObject Robot;
    public float robotRotationAngle = 360f;
    public float robotRotationTime = 20f;

    private GameStateSO gameStateSO;
    private PlayerStatsSO initialPlayerStat;
    private Tween rotationTween;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        initialPlayerStat = GameManager.Instance.FetchInitialPlayerStat();
        gameStateSO = GameManager.Instance.FetchGameStateData();
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.2f);
        TabButton homeButton = tabButtons.Find(button => button.name == "Home");
        TabButton upgradeButton = tabButtons.Find(button => button.name == "Upgrade");

        if (homeButton != null && gameStateSO.isDefaultHomeButton)
        {
            OnTabSelected(homeButton); 
        }
        
        if(upgradeButton != null && !gameStateSO.isDefaultHomeButton)
        {
            OnTabSelected(upgradeButton);
        }
    }

    public void OnTabEnter(TabButton button)
    {
        if (timerState == ButtonTimerState.Ready)
        {
            ResetButtons();
            if (_selectedTab == null || button != _selectedTab)
            {
                
            }
        }
    }

    public void OnTabExit(TabButton button)
    {
        if (timerState == ButtonTimerState.Ready)
        {
            ResetButtons();
        }
    }

    private IEnumerator ButtonCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        timerState = ButtonTimerState.Ready;
    }

    public void OnTabSelected(TabButton button)
    {
        if (timerState != ButtonTimerState.Ready) return;

        _selectedTab = button;
        tabIsOpen = true;
        ResetButtons();

        // tween animation
        button.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.1f) // selected scale size
            .SetEase(Ease.OutCubic)
            .OnStepComplete(() => StartCoroutine(ButtonCooldown()));
        //button.transform.DOLocalMoveY(-70f, 0.1f);

        timerState = ButtonTimerState.Waiting;
        PageActivation(button);
    }

    private void ResetButtons()
    {
        foreach (TabButton button in tabButtons)
        {
            if (_selectedTab != null && button == _selectedTab) continue;

            if (timerState == ButtonTimerState.Ready)
            {
                button.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0.1f) // reset scale size
                    .SetEase(Ease.OutCubic);
                //button.transform.DOLocalMoveY(-84f, 0.1f);
            }
        }
    }

    private void PageActivation(TabButton button)
    {
        switch (button.name)
        {
            case "Home":
                OpenHomePage();
                break;
            case "Upgrade":
                OpenUpgradePage();
                break;
            default:
                Debug.LogWarning("Unknown Tab Selected");
                Robot.transform.rotation = Quaternion.identity;
                rotationTween.Kill();
                break;
        }
    }

    private void ActivatePageByIndex(int index)
    {
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
                objectsToSwap[i].transform.DOLocalMoveX(0, 0.5f)
                    .SetEase(Ease.InQuint)
                    .OnStepComplete(() => Debug.Log($"Page {i} activated"));
            }
            else
            {
                objectsToSwap[i].transform.DOLocalMoveX(2044f, 0.1f)
                    .SetEase(Ease.InQuint)
                    .OnStepComplete(() =>
                    {
                        Debug.Log($"Page {i} deactivated");
                    });
            }
        }
    }

    public void OpenHomePage()
    {
        ActivatePageByIndex(0);
        rotationTween = Robot.transform.DORotate(new Vector3(0, robotRotationAngle, 0), robotRotationTime, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart); ;
    }

    public void OpenUpgradePage()
    {
        ActivatePageByIndex(1);
        Robot.transform.rotation = Quaternion.identity;
        rotationTween.Kill();
    }

}

public enum ButtonTimerState
{
    Ready,
    Waiting,
    Cooldown
}
