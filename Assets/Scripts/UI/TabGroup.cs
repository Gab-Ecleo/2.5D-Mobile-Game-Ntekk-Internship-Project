using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public List<GameObject> objectsToSwap;

    [Header("Sprite")]
    public Sprite tabIdle;
    public Sprite tabActive;
    public Sprite tabHover;
    public Sprite tabExit;

    private TabButton _selectedTab;
    public bool tabIsOpen;
    public ButtonTimerState timerState = ButtonTimerState.Ready;
    public float cooldownTime = 1f;

    public int index;

    public GameObject Robot;
    public float robotRotationAngle = 360f;
    public float robotRotationTime = 20f;
    private Tween rotationTween;
    private void Start()
    {
        Time.timeScale = 1.0f; // delete when there's new toggle pause logic in pausemanager 
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.2f);
        TabButton defaultButton = tabButtons.Find(button => button.name == "Home");
        if (defaultButton != null)
        {
            OnTabSelected(defaultButton);
            
        }
    }

    public void OnTabEnter(TabButton button)
    {
        if (timerState == ButtonTimerState.Ready)
        {
            ResetButtons();
            if (_selectedTab == null || button != _selectedTab)
            {
                button.Background.sprite = tabHover;
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
        button.Background.sprite = tabActive;

        // tween animation
        button.transform.DOScale(new Vector3(0.9f, 0.8f, 0.6f), 0.1f)
            .SetEase(Ease.OutCubic)
            .OnStepComplete(() => StartCoroutine(ButtonCooldown()));
        button.transform.DOLocalMoveY(-138f, 0.1f);

        PageActivation(button);
        timerState = ButtonTimerState.Waiting;
    }

    public void ResetButtons()
    {
        foreach (TabButton button in tabButtons)
        {
            if (_selectedTab != null && button == _selectedTab) continue;
            button.Background.sprite = tabIdle;

            if (timerState == ButtonTimerState.Ready)
            {
                button.transform.DOScale(new Vector3(0.8f, 0.6f, 0.6f), 0.1f)
                    .SetEase(Ease.OutCubic);
                button.transform.DOLocalMoveY(-158f, 0.1f);
            }
        }
    }

    public void PageActivation(TabButton button)
    {
        switch (button.name)
        {
            case "Home":
                ActivatePageByIndex(0);
                rotationTween = Robot.transform.DORotate(new Vector3(0, robotRotationAngle, 0), robotRotationTime, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart); ;
                break;
            case "Upgrade":
                ActivatePageByIndex(1);
                Robot.transform.rotation = Quaternion.identity;
                rotationTween.Kill();
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
                objectsToSwap[i].transform.DOLocalMoveX(1400f, 0.1f)
                    .SetEase(Ease.InQuint)
                    .OnStepComplete(() =>
                    {
                        Debug.Log($"Page {i} deactivated");
                    });
            }
        }
    }


}

public enum ButtonTimerState
{
    Ready,
    Waiting,
    Cooldown
}
