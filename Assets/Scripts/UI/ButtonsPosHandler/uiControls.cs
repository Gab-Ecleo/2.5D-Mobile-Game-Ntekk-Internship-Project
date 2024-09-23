using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class uiControls : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public ButtonSO buttonSO;

    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform confiner;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private float objectWidth;
    private float objectHeight;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        objectWidth = rectTransform.rect.width;
        objectHeight = rectTransform.rect.height;
    }

    private void Start()
    {
        if (buttonSO == null || rectTransform == null) { return; }
        // ConfineToBounds();
    }

    private void ConfineToBounds()
    {
        Vector3[] confinerCorners = new Vector3[4];
        confiner.GetWorldCorners(confinerCorners);

        Vector3 minBound = confinerCorners[0];
        Vector3 maxBound = confinerCorners[2];

        Vector3 viewPos = rectTransform.position;

        float halfWidth = objectWidth / 2;
        float halfHeight = objectHeight / 2;

        viewPos.x = Mathf.Clamp(viewPos.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        rectTransform.position = viewPos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        ConfineToBounds();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        buttonSO.inIntialPos = false;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        buttonSO.CurrPos = rectTransform.position;
    }


    private bool RectOverlaps(RectTransform rect1, RectTransform rect2)
    {
        Rect rect1World = GetWorldRect(rect1);
        Rect rect2World = GetWorldRect(rect2);

        return rect1World.Overlaps(rect2World);
    }

    private Rect GetWorldRect(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        Vector3 topLeft = corners[0];
        Vector3 bottomRight = corners[2];

        return new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);
    }

}
