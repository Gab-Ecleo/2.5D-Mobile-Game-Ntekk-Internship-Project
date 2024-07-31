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

    #region Start Game Button
    // make sure that tab 0 is the Home/Player button
    private void Start()
    {
        if (tabButtons != null && tabButtons.Count > 0)
        {
            OnTabSelected(tabButtons[0], false);
        }
    }
    #endregion

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetButtons();
        if (_selectedTab == null || button != _selectedTab) { button.Background.sprite = tabHover; }
        
    }
    public void OnTabExit(TabButton button)
    {
        ResetButtons();
    }
    public void OnTabSelected(TabButton button, bool isAnimated)
    {
        _selectedTab = button;
        ResetButtons();
        button.Background.sprite = tabActive;

        if (isAnimated)
        {
            LeanTween.scale(button.gameObject, new Vector3(0.9f, 0.8f, 0.6f), 0.1f).setDelay(0.1f).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveLocalY(button.gameObject, -138f, 0.1f);
        }
        else
        {
            button.transform.localScale = new Vector3(0.9f, 0.8f, 0.6f);
            button.transform.localPosition = new Vector3(button.transform.localPosition.x, -138f, button.transform.localPosition.z);
        }

        ActivatePage(button, isAnimated);
    }

    public void ResetButtons()
    {
        foreach(TabButton button in tabButtons)
        {
            if(_selectedTab != null && button == _selectedTab) { continue; }
            button.Background.sprite = tabIdle;

            // reset the scale and tranform
            LeanTween.scale(button.gameObject, new Vector3(0.8f, 0.6f, 0.6f), 0.1f).setDelay(0.1f).setEase(LeanTweenType.easeOutCubic);
            LeanTween.moveLocalY(button.gameObject, -158f, 0.1f);
        }
    }

    public void ActivatePage(TabButton button, bool animate)
    {
        // get the pages for each button
        // make sure the page list has the same arrangement as the button list
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                if (animate)
                {
                    LeanTween.moveLocalX(objectsToSwap[i], 0, 0.5f).setDelay(0.1f).setEase(LeanTweenType.easeInQuint);
                }
                else
                {
                    objectsToSwap[i].transform.localPosition = new Vector3(0f, objectsToSwap[i].transform.localPosition.y, objectsToSwap[i].transform.localPosition.z);    
                }
            }
            else
            {
                LeanTween.moveLocalX(objectsToSwap[i], 1400f, 0.1f);
            }
        }
    }
}
