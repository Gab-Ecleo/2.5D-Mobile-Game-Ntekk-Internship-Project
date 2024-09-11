using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class uiControls : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private RectTransform confiner;
    [SerializeField] public ButtonSO buttonSO;

    private RectTransform m_RectTransform;
    private CanvasGroup m_Group;

    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Group = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (buttonSO == null || m_RectTransform == null) { return; }
    }

    
    private void ConfineToBounds()
    {
        Vector3[] confinerCorners = new Vector3[4];
        confiner.GetWorldCorners(confinerCorners);

        Vector3 minBound = confinerCorners[0];
        Vector3 maxBound = confinerCorners[2];

        Vector3 viewPos = m_RectTransform.position;

        float halfWidth = m_RectTransform.rect.width / 2;
        float halfHeight = m_RectTransform.rect.height / 2;

        viewPos.x = Mathf.Clamp(viewPos.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, minBound.y + halfHeight, maxBound.y - halfHeight);

        m_RectTransform.position = viewPos;
    }

    private void SendDataToSO(Vector3 pos)
    {
        buttonSO.CurrPos = pos;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_Group.blocksRaycasts = false;
        m_Group.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        ConfineToBounds();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_Group.blocksRaycasts = true;
        m_Group.alpha = 1;
    }


    /// <summary>
    ///  can acess the save pos of each button here
    /// </summary>
    public void SaveData()
    {
        SendDataToSO(m_RectTransform.position);
    }
}
