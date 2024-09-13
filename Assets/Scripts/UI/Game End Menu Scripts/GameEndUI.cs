using System;
using System.Globalization;
using ScriptableData;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;

namespace UI.Game_End_Menu_Scripts
{
    public class GameEndUI : MonoBehaviour
    {
        public GameObject GameOverGO;

        [Header("Scriptable Data")]
        [SerializeField] private ScoresSO scoreData;
        [SerializeField] private CurrencySO currencyData;

        [Header("Main Panel Anim")]
        [SerializeField] private float tweenDuration = 1f;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform bgRectTransform;
        [SerializeField] private RectTransform mainRectTrans;
        [SerializeField] private Ease easeIn, easeOut;

        [Header("Title Anim")]
        [SerializeField] private RectTransform panelTitleHolder;
        [SerializeField] private float titleDuration = 2f;

        [Header("Score Anim")]
        [SerializeField] private RectTransform scoreRectTrans;
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private int currentScore;

        [Header("Currency Anim")]
        [SerializeField] private RectTransform currencyTrans;
        [SerializeField] private TextMeshProUGUI CurrencyText;
        [SerializeField] private int currentCurrency;

        [Header("Buttons Anim")]
        [SerializeField] private RectTransform buttonsRectTrans;

        private Sequence introTween;
        private bool isAnimating = false;
        private void OnEnable()
        {
            GameEvents.TRIGGER_GAMEEND_SCREEN += PanelIntro;
            GameEvents.TRIGGER_END_OF_GAMEEND_SCREEN += PlayOutro;
            GameEvents.COMPLETE_TWEEN += CompleteAllTween;
        }

        private void OnDisable()
        {
            GameEvents.TRIGGER_GAMEEND_SCREEN -= PanelIntro;
            GameEvents.TRIGGER_END_OF_GAMEEND_SCREEN -= PlayOutro;
            GameEvents.COMPLETE_TWEEN -= CompleteAllTween;
        }

        private void Start()
        {
            isAnimating = false;
            GameOverGO.SetActive(isAnimating);
            //PanelIntro(true); // for testing
        }

        #region tween anim

        public void PanelIntro(bool _isAnimating)
        {
            if(_isAnimating)
            {
                isAnimating = _isAnimating;
                GameOverGO.SetActive(isAnimating);

                Time.timeScale = 0;
                introTween = DOTween.Sequence();

                introTween.Append(canvasGroup.DOFade(1, tweenDuration).SetUpdate(true))
                          .Append(bgRectTransform.DOLocalMoveY(908, tweenDuration).SetUpdate(true).SetEase(easeIn))
                          .Append(panelTitleHolder.DOLocalMoveY(1181, tweenDuration).SetUpdate(true).SetEase(easeIn))
                          .Append(scoreRectTrans.DOLocalMoveY(1150, tweenDuration).SetUpdate(true).SetEase(easeIn))
                          .Append(currencyTrans.DOLocalMoveY(950, tweenDuration).SetUpdate(true).SetEase(easeIn))
                          .Append(buttonsRectTrans.DOLocalMoveY(550, tweenDuration).SetUpdate(true).SetEase(easeIn))
                          .OnComplete(() => {
                              ScoreDisplay();
                              CurrencyDisplay();
                          }).SetUpdate(true);
            }
            else
            {
                isAnimating = false;
            }
        }

        private Tween TypewriterTween(TMP_Text _text)
        {
            int titleLength = _text.text.Length;

            Sequence typewriterSequence = DOTween.Sequence();
            for (int i = 0; i < titleLength; i++)
            {
                int charIndex = i;
                typewriterSequence.Append(DOVirtual.DelayedCall((titleDuration / titleLength) * charIndex, () =>
                {
                    _text.maxVisibleCharacters = charIndex + 1;
                }).SetUpdate(true));
            }

            return typewriterSequence;
        }

        private void ScoreDisplay()
        {
            DOVirtual.Int(currentScore, scoreData.Points, 1, (x) =>
            {
                ScoreText.text = x.ToString("D4");
            }).SetUpdate(true);
        }

        private void CurrencyDisplay()
        {
            DOVirtual.Int(currentCurrency, currencyData.matchCoins, 1, (y) =>
            {
                CurrencyText.text = y.ToString("D4"); 
            }).SetUpdate(true);
        }


        public async void PlayOutro()
        {
            await PanelOutro();
        }

        async Task PanelOutro()
        {
            Time.timeScale = 1;
            canvasGroup.alpha = 1f;
            canvasGroup.DOFade(0, tweenDuration).SetUpdate(true);
            await mainRectTrans.DOMoveY(-1249f, tweenDuration).SetUpdate(true).SetEase(easeOut).AsyncWaitForCompletion();
        }

        #endregion

        private void CompleteAllTween()
        {
            if(isAnimating) 
                introTween.Complete(true);
            
        }

    }


}
