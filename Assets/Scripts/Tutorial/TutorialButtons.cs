using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TutorialButtons : MonoBehaviour,IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TutorialLock tutorialLock;
    public Image Background;
    public Button button;

    public bool isInteractable;

    private void Awake()
    {
        Background = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isInteractable)
            tutorialLock.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tutorialLock.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tutorialLock.OnTabExit(this);
    }

    public void SetInteractable(bool interactable)
    {
        isInteractable = interactable;
        button.interactable = interactable;
    }
}