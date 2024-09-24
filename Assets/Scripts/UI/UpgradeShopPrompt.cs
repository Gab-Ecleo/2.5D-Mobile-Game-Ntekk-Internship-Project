using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UpgradeShop;

public class UpgradeShopPrompt : MonoBehaviour
{
    public UpgradeShopUIManager upgradeShopUI;
    
    [SerializeField] private RectTransform popUp;
    [SerializeField] private Ease ease;
    [SerializeField] private float duration;
    [SerializeField] private float topY,midY;
    private bool isOpen;
    private bool isThisHomeButton;
    

    private async void TogglePopUp()
    {
        if (!isOpen)
        {
            isOpen = true;
            Time.timeScale = 0;

            PopUpIntro();
        }
        else
        {
            await PopUpOutro();
            isOpen = false;

            Time.timeScale = 1;
        }
    }

    private void PopUpIntro()
    {
        popUp.DOAnchorPosY(midY, duration).SetUpdate(true);
    }

    async Task PopUpOutro()
    {
        await popUp.DOAnchorPosY(topY, duration).SetUpdate(true).AsyncWaitForCompletion();
    }

    public void FirstLayerButton(bool _isThisHomeButton)
    {
        isThisHomeButton = _isThisHomeButton;
        if (upgradeShopUI.NoChanges)
        {
            YesOrNoPopupButton();
        }
        else
        {
            TogglePopUp();
        }
    }


    public void YesOrNoPopupButton()
    {
        if (isThisHomeButton)
        {
            SceneController.Instance.LoadScene(0);
        }
        else
        {
            SceneController.Instance.LoadScene(2); 
        }
    }

    public void Resume()
    {
        TogglePopUp();
    }
}
