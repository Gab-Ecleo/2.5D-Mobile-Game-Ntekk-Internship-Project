using System;
using System.Globalization;
using ScriptableData;
using TMPro;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using System.Collections;

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
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Ease easeIn, easeOut;
        [SerializeField] private float midY, botY;

        [Header("Title Anim")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private float titleDuration = 2f;

        [Header("Score Anim")]
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private int currentScore;
        [SerializeField] private float scoreDuration = 1f;

        [Header("Currency Anim")]
        [SerializeField] private TextMeshProUGUI CurrencyText;
        [SerializeField] private int currentCurrency;
        [SerializeField] private float currencyDuration = 1f;

        private void OnEnable()
        {
            GameEvents.TRIGGER_GAMEEND_SCREEN += PanelIntro;
            GameEvents.TRIGGER_END_OF_GAMEEND_SCREEN += PlayOutro;
        }

        private void OnDisable()
        {
            GameEvents.TRIGGER_GAMEEND_SCREEN -= PanelIntro;
            GameEvents.TRIGGER_END_OF_GAMEEND_SCREEN -= PlayOutro;
        }

        private void Start()
        {
            titleText.maxVisibleCharacters = 0;
            GameOverGO.SetActive(false);
            //PanelIntro(); // for testing
        }

        #region tween anim

        private void PanelIntro()
        {
            GameOverGO.SetActive(true);
            Time.timeScale = 0;
            Sequence introTween = DOTween.Sequence();

            introTween.Append(rectTransform.DOAnchorPosY(midY, tweenDuration).SetUpdate(true).SetEase(easeIn))
                      .Append(canvasGroup.DOFade(1, tweenDuration).SetUpdate(true))
                      .OnComplete(() =>{TypewriterEffect();})
                      .AppendCallback(() => ScoreDisplay()).SetUpdate(true); ;
        }

        private void ScoreDisplay()
        {
            ScoreText.text = currentScore.ToString("D5");

            DOVirtual.Int(currentScore, scoreData.Points, scoreDuration, (x) =>
            {
                ScoreText.text = x.ToString("D5");
            }).SetUpdate(true).OnComplete(() =>
            {
                CurrencyDisplay(); 
            });
        }

        private void CurrencyDisplay()
        {
            DOVirtual.Int(currentCurrency, currencyData.matchCoins, currencyDuration, (y) =>
            {
                CurrencyText.text = y.ToString("D5"); 
            }).SetUpdate(true);
        }

        private void TypewriterEffect()
        {
            int titleLength = titleText.text.Length;
            for (int i = 0; i < titleLength; i++)
            {
                int charIndex = i;
                DOVirtual.DelayedCall(titleDuration * i / titleLength, () =>
                {
                    titleText.maxVisibleCharacters = charIndex + 1;
                }).SetUpdate(true);
            }
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
            await rectTransform.DOAnchorPosY(botY, tweenDuration).SetUpdate(true).SetEase(easeOut).AsyncWaitForCompletion();
        }

        #endregion
    }
}
