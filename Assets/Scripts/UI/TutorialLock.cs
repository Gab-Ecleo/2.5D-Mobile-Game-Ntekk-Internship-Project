using ScriptableData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Enable this only when player's first game 

public class TutorialLock : MonoBehaviour, IPointerClickHandler
{
    [Header("Arrange them the same")]
    [SerializeField] private SwipeController[] swipeController;
    [SerializeField] private Button[] buttons;
    [SerializeField] private GameObject[] panelsGO;
    [SerializeField] private Button exitButton;

    [Header("Sprite")]
    [SerializeField] private Sprite tabIdle;
    [SerializeField] private Sprite tabActive;
    [SerializeField] private Sprite tabHover;

    private PlayerStatsSO initialPlayerStat;

    private bool _isPlayerFirstGame;

    private bool _buttonIsActive;

    private void Start()
    {
        initialPlayerStat = GameManager.Instance.FetchInitialPlayerStat();

        if (!initialPlayerStat.stats.isPlayerFirstGame) { return; }

        foreach (var button in buttons)
        {
            button.interactable = false;
        }

        // the first in the array should be hazard and the only one interactable on every start
        if (buttons.Length > 0)
        {
            buttons[0].interactable = true;
        }

        exitButton.interactable = false;
    }

    private void OnEnable()
    {
        GameEvents.ON_TUTORIAL_UNLOCKED += UnlockTutorial;
    }
    private void OnDisable()
    {
        GameEvents.ON_TUTORIAL_UNLOCKED -= UnlockTutorial;
    }


    private void UnlockTutorial()
    { 
        if (!initialPlayerStat.stats.isPlayerFirstGame) { return; }

        for (int i = 0; i < swipeController.Length; i++)
        {
            if (swipeController[i].isTutorialDone)
            {
                Debug.Log($"Tutorial step {i} completed.");

                switch (i)
                {
                    case 0:
                        buttons[1].interactable = true;
                        Debug.Log("Hazard unlocked, Powerups button interactable.");
                        break;
                    case 1:
                        buttons[2].interactable = true;
                        Debug.Log("Powerups unlocked, Blocks button interactable.");
                        break;
                    case 2:
                        buttons[3].interactable = true;
                        Debug.Log("Blocks unlocked, Win/Lose button interactable.");
                        break;
                    case 3:
                        Debug.Log("Win/Lose unlocked.");

                        // After the last tutorial you can exit the panel and update the stats
                        exitButton.interactable = true;
                        initialPlayerStat.stats.isPlayerFirstGame = false;

                        // delete after testing 
                        Debug.Log("isPlayerFirstGame: " + initialPlayerStat.stats.isPlayerFirstGame); 
                        break;
                    default:
                        Debug.LogWarning("Something went wrong");
                        break;
                }
            }
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Button clickedButton = eventData.pointerPress.GetComponent<Button>();

        if (clickedButton != null)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i] == clickedButton)
                {
                    buttons[i].image.sprite = tabActive;
                    panelsGO[i].gameObject.SetActive(true);
                }
                else
                {
                    buttons[i].image.sprite = tabIdle;
                    if (i < panelsGO.Length)
                    {
                        panelsGO[i].gameObject.SetActive(false);
                    }
                }
            }
        }
    }

}
