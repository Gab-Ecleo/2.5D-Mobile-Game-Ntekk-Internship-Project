using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [Header("Pages")]
    [SerializeField] private int maxPages;
    [SerializeField] private Vector3 pageStep;
    [SerializeField] private RectTransform imgPagesRect;

    [Header("Buttons")]
    [SerializeField] private Button rightButton;
    [SerializeField] private Button leftButton;

    [Header("Tween")]
    [SerializeField] private float tweenTime;
    [SerializeField] private Ease tweenType;

    [Header("Footer")]
    [SerializeField] private Image[] footerImg;
    [SerializeField] private Sprite footerClosed;
    [SerializeField] private Sprite footerOpen;

    private int currPages;
    private Vector3 targetPos;
    private float dragThreshhold;

    public bool isTutorialDone { get; private set; }

    private void Awake()
    {
        currPages = 1;
        targetPos = imgPagesRect.localPosition;

        Debug.Log(imgPagesRect.localPosition);
        UpdateFooterImg();
        UpdateArrowButton();
    }

    public void ResetContent()
    {
        currPages = 1;
        targetPos = Vector3.zero; 
        imgPagesRect.localPosition = targetPos; 
        UpdateFooterImg();
        UpdateArrowButton();
    }


    #region Content Handler
    public void NextButton()
    {
        if(currPages < maxPages)
        {
            currPages++;
            targetPos += pageStep;
            MovePage();
        }
    }

    public void PrevButton()
    {
        if (currPages > 1)
        {
            currPages--;
            targetPos -= pageStep;
            MovePage();
        }
    }

    private void UpdateArrowButton()
    {
        rightButton.interactable = true;
        leftButton.interactable = true;

        if (currPages == 1 )
        {
            isTutorialDone = false;
            leftButton.interactable = false;
        }
        else if (currPages == maxPages)
        {
            rightButton.interactable = false;
            isTutorialDone = true;
            Debug.Log("Tutorial Done");

            // unlcok tutorial every last page of each tutorial section 
            GameEvents.ON_TUTORIAL_UNLOCKED?.Invoke(); 
        }
    }

    private void MovePage()
    {
        UpdateFooterImg();
        UpdateArrowButton();
        imgPagesRect.DOLocalMove(targetPos, tweenTime).SetUpdate(true).SetEase(tweenType);
    }
    #endregion

    #region Swipe Handler
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshhold)
        {
            if (eventData.position.x > eventData.pressPosition.x) 
                PrevButton();
            else 
                NextButton();
        }
        else
        {
            MovePage();
        }
    }

    #endregion

    #region Footer
    private void UpdateFooterImg()
    {
        foreach (var item in footerImg)
        {
            item.sprite = footerClosed;
        }
        footerImg[currPages - 1].sprite = footerOpen;
    }
    #endregion
}
