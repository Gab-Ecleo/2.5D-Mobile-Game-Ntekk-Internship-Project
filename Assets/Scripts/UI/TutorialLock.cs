using DG.Tweening;
using ScriptableData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventScripts;

public class TutorialLock : MonoBehaviour
{

    [Header("Arrange them the same")]
    [SerializeField] private SwipeController[] swipeController;

    [SerializeField] private List<TutorialButtons> buttonsList;
    [SerializeField] private GameObject[] panelsGO;
    [SerializeField] private Button exitButton;

    [Header("Sprite")]
    [SerializeField] private Sprite buttonIdle;
    [SerializeField] private Sprite buttonSelected;
    [SerializeField] private Sprite buttonDisabled;
    [SerializeField] private Sprite buttonHover;

    [SerializeField] private GameStateSO gameStateSO;
    public TutorialButtons defaultButton;
    private TutorialButtons _selectedTab;

    private void Start()
    {
        gameStateSO = GameManager.Instance.FetchGameStateData();

        if (!gameStateSO.isPlayerFirstGame) return;

        // Initialize buttons
        foreach (var button in buttonsList)
        {
            button.SetInteractable(false);
            button.Background.sprite = buttonDisabled;
        }

        defaultButton.Background.sprite = buttonSelected;

        exitButton.interactable = false;
    }

    private void OnEnable()
    {
        GameEvents.ON_TUTORIAL_UNLOCKED += UnlockTutorial;
        defaultButton.Background.sprite = buttonSelected;
    }

    private void OnDisable()
    {
        GameEvents.ON_TUTORIAL_UNLOCKED -= UnlockTutorial;
        for (int i = 0; i < buttonsList.Count; i++)
        {
            defaultButton.Background.sprite = buttonIdle;
        }
    }

    private void UnlockTutorial()
    {
        if (!gameStateSO.isPlayerFirstGame)
        {
            for (int i = 0; i < buttonsList.Count; i++)
            {
                ActivateButton(i);
            }
            Debug.Log("Unlock All Buttons");
            exitButton.interactable = true;
        }

        for (int i = 0; i < swipeController.Length; i++)
        {
            if (swipeController[i].isTutorialDone)
            {
                Debug.Log($"Tutorial step {i} completed.");

                switch (i)
                {
                    case 0:
                        ActivateButton(1);
                        Debug.Log("Hazard unlocked, Powerups button interactable.");
                        break;
                    case 1:
                        ActivateButton(2);
                        Debug.Log("Powerups unlocked, Blocks button interactable.");
                        break;
                    case 2:
                        ActivateButton(3);
                        Debug.Log("Blocks unlocked, Win/Lose button interactable.");
                        break;
                    case 3:
                        exitButton.interactable = true;
                        gameStateSO.isPlayerFirstGame = false;
                        LocalStorageEvents.OnSaveGameStateData?.Invoke();
                        break;
                    default:
                        Debug.LogWarning("Something went wrong");
                        break;
                }
            }
        }
    }

    private void ActivateButton(int index)
    {
        if (index >= 0 && index < buttonsList.Count)
        {
            buttonsList[index].SetInteractable(true);
        }
    }
    public void OnTabEnter(TutorialButtons button)
    {
        if (_selectedTab == null || button != _selectedTab)
        {
            button.Background.sprite = buttonHover;
        }
    }


    public void OnTabExit(TutorialButtons button)
    {
        if (_selectedTab != null && button != _selectedTab)
        {
            button.Background.sprite = buttonIdle;
        }
    }

    public void OnTabSelected(TutorialButtons button)
    {
        _selectedTab = button;
        ResetButtons();
        button.Background.sprite = buttonSelected;
        PageActivation(button);
    }

    public void ResetButtons()
    {
        foreach (TutorialButtons button in buttonsList)
        {
            if (_selectedTab != null && button == _selectedTab) continue;
            button.Background.sprite = buttonIdle;
        }
    }

    public void PageActivation(TutorialButtons button)
    {
        int index = buttonsList.IndexOf(button);
        if (index >= 0 && index < panelsGO.Length)
        {
            for (int i = 0; i < panelsGO.Length; i++)
            {
                panelsGO[i].SetActive(i == index);
            }
        }
    }
}
