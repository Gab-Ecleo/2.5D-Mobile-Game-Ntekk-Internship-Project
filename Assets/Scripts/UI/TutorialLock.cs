using ScriptableData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialLock : MonoBehaviour
{
    [Header("Arrange them the same")]
    [SerializeField] private SwipeController[] swipeController;
    [SerializeField] private Button[] buttons;

    PlayerStatsSO currPlayerStats;

    private bool _isPlayerFirstGame;

    private bool _isHazardUnlocked;
    private bool _isPowerupsUnlocked;
    private bool _isBlocksUnlocked;
    private bool _isWinLoseUnlocked;

    private void Start()
    {
        currPlayerStats = GameManager.Instance.FetchCurrentPlayerStat();

        foreach (var button in buttons)
        {
            button.interactable = false;
        }

        // the first in the array should be hazard and the only one interactable on every start
        if (buttons.Length > 0)
        {
            buttons[0].interactable = true;
        }
    }

    private void Update()
    {
        UnlockTutorial(); // to be revised this is for testing only
    }

    private void UnlockTutorial()
    {
        _isPlayerFirstGame = currPlayerStats.stats.isPlayerFirstGame;

        //if (_isPlayerFirstGame) { return; }

        for (int i = 0; i < swipeController.Length; i++)
        {
            if (swipeController[i].isTutorialDone)
            {
                Debug.Log($"Tutorial step {i} completed.");

                switch (i)
                {
                    case 0:
                        _isHazardUnlocked = true;
                        buttons[1].interactable = true;
                        Debug.Log("Hazard unlocked, Powerups button interactable.");
                        break;
                    case 1:
                        _isPowerupsUnlocked = true;
                        buttons[2].interactable = true;
                        Debug.Log("Powerups unlocked, Blocks button interactable.");
                        break;
                    case 2:
                        _isBlocksUnlocked = true;
                        buttons[3].interactable = true;
                        Debug.Log("Blocks unlocked, Win/Lose button interactable.");
                        break;
                    case 3:
                        _isWinLoseUnlocked = true;
                        Debug.Log("Win/Lose unlocked.");
                        break;
                    default:
                        Debug.LogWarning("Something went wrong");
                        break;
                }
                UpdatePlayerStats();
            }
        }
    }

    private void UpdatePlayerStats()
    {
        currPlayerStats.stats.isHazardUnlocked = _isHazardUnlocked;
        currPlayerStats.stats.isPowerupsUnlocked = _isPowerupsUnlocked;
        currPlayerStats.stats.isBlocksUnlocked = _isBlocksUnlocked;
        currPlayerStats.stats.isWinLoseUnlocked = _isWinLoseUnlocked;
    }
}
