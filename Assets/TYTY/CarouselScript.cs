using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CarouselScript : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private HorizontalLayoutGroup Content;
    [SerializeField] private Camera Camera;

    [Header("Arrow Keys")]
    [SerializeField] private Button LeftButton;
    [SerializeField] private Button RightButton;

    [Header("Carousel Setup")]
    [SerializeField] private int CurrentIndex = 0;
    /// the number of items in the carousel that should be moved every time
    [SerializeField] private int Pagination = 1;
    /// the percentage of distance that, when reached, will stop movement
    [SerializeField] private float ThresholdInPercent = 1f;
    [SerializeField] private float MoveSpeed = 0.05f;

    [Header("Focus")]
    /// Bind here the carousel item that should have focus initially
    public GameObject InitialFocus;


    protected float _elementWidth;
    protected int _contentLength = 0;
    protected float _spacing;
    protected Vector2 _initialPosition;
    protected RectTransform _rectTransform;

    protected bool _lerping = false;
    protected float _lerpStartedTimestamp;
    protected Vector2 _startPosition;
    protected Vector2 _targetPosition;

    // Start is called before the first frame update
    public void Start()
    {
        _rectTransform = Content.gameObject.GetComponent<RectTransform>();
        _initialPosition = _rectTransform.anchoredPosition;

        // we compute the Content's element width
        _contentLength = 0;
        foreach (Transform tr in Content.transform)
        {
            _elementWidth = tr.GetComponent<RectTransform>().sizeDelta.x;
            _contentLength++;
        }
        _spacing = Content.spacing;

        // we position our carousel at the desired initial index
        _rectTransform.anchoredPosition = DeterminePosition();

        if (InitialFocus != null)
        {
            EventSystem.current.SetSelectedGameObject(InitialFocus, null);
        }
    }

    public void MoveLeft()
    {
        if (!CanMoveLeft()) { return; }
        else
        {
            CurrentIndex -= Pagination;
            MoveToCurrentIndex();
        }
    }
    public void MoveRight()
    {
        if (!CanMoveRight()) { return; }
        else
        {
            CurrentIndex += Pagination;
            MoveToCurrentIndex();
        }
    }

    public void MoveToCurrentIndex()
    {
        _startPosition = _rectTransform.anchoredPosition;
        _targetPosition = DeterminePosition();
        _lerping = true;
        _lerpStartedTimestamp = Time.time;
    }

    public Vector2 DeterminePosition()
    {
        return _initialPosition - (Vector2.right * CurrentIndex * (_elementWidth + _spacing));
    }

    public bool CanMoveLeft()
    {
        return (CurrentIndex - Pagination >= 0);
    }

    public bool CanMoveRight()
    {
        return (CurrentIndex + Pagination < 0);
    }

    public void Update()
    {
        if (_lerping)
        {
            LerpPosition();
        }
        HandleButtons();
        HandleFocus();
    }

    public void HandleFocus()
    {
        if (!_lerping && Time.timeSinceLevelLoad > 0.5f)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                if (Camera.WorldToScreenPoint(EventSystem.current.currentSelectedGameObject.transform.position).x < 0)
                {
                    MoveLeft();
                }
                if (Camera.WorldToScreenPoint(EventSystem.current.currentSelectedGameObject.transform.position).x > Screen.width)
                {
                    MoveRight();
                }
            }
        }
    }

    public void HandleButtons()
    {
        if (LeftButton != null)
        {
            if (CanMoveLeft())
            {
                LeftButton.interactable = true;
            }
            else
            {
                LeftButton.interactable = false;
            }
        }
        if (RightButton != null)
        {
            if (CanMoveRight())
            {
                RightButton.interactable = true;
            }
            else
            {
                RightButton.interactable = false;
            }
        }
    }

    public void LerpPosition()
    {
        float timeSinceStarted = Time.time - _lerpStartedTimestamp;
        float percentageComplete = timeSinceStarted / MoveSpeed;

        _rectTransform.anchoredPosition = Vector2.Lerp(_startPosition, _targetPosition, percentageComplete);

        //When we've completed the lerp, we set _isLerping to false
        if (percentageComplete >= ThresholdInPercent)
        {
            _lerping = false;
        }
    }
}
