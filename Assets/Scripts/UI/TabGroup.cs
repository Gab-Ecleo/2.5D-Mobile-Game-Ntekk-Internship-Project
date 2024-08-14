using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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

    private void Start()
    {
        if (tabButtons != null && tabButtons.Count > 0)
        {
            OnTabSelected(tabButtons[0]);
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

    public void OnTabSelected(TabButton button)
    {
        if (timerState != ButtonTimerState.Ready) return;

        if (_selectedTab == button) return;

        _selectedTab = button;
        tabIsOpen = true;
        ResetButtons();
        button.Background.sprite = tabActive;

        LeanTween.scale(button.gameObject, new Vector3(0.9f, 0.8f, 0.6f), 0.1f)
                 .setDelay(0.1f).setEase(LeanTweenType.easeOutCubic);
        LeanTween.moveLocalY(button.gameObject, -138f, 0.1f);

        ActivatePage(button);

        StartCoroutine(ButtonCooldown());
    }

    public void ResetButtons()
    {
        foreach (TabButton button in tabButtons)
        {
            if (_selectedTab != null && button == _selectedTab) continue;
            button.Background.sprite = tabIdle;

            LeanTween.scale(button.gameObject, new Vector3(0.8f, 0.6f, 0.6f), 0.1f)
                     .setDelay(0.1f).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveLocalY(button.gameObject, -158f, 0.1f);
        }
    }

    public void ActivatePage(TabButton button)
    {
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                LeanTween.moveLocalX(objectsToSwap[i], 0, 0.5f)
                         .setEase(LeanTweenType.easeInQuint);
            }
            else
            {
                LeanTween.moveLocalX(objectsToSwap[i], 1400f, 0.1f)
                         .setEase(LeanTweenType.easeInQuint);
            }
        }
    }

    private IEnumerator ButtonCooldown()
    {
        timerState = ButtonTimerState.Waiting;
        yield return new WaitForSeconds(cooldownTime);
        timerState = ButtonTimerState.Ready;
    }
}

public enum ButtonTimerState
{
    Ready,
    Waiting,
    Cooldown
}

