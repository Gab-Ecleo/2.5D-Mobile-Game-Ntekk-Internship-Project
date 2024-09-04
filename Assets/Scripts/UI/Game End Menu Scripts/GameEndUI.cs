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
        [SerializeField] private RectTransform bgRectTransform;
        [SerializeField] private RectTransform mainRectTrans;
        [SerializeField] private Ease easeIn, easeOut;

        [Header("Title Anim")]
        [SerializeField] private RectTransform panelTitleHolder;
        [SerializeField] private TMP_Text panelTitleText;
        [SerializeField] private float titleDuration = 2f;

        [Header("Score Anim")]
        [SerializeField] private RectTransform finalScorePanel;
        [SerializeField] private TextMeshProUGUI ScoreTitleText;
        [SerializeField] private CanvasGroup ScoreNumberTextCanvasGrp;
        [SerializeField] private TextMeshProUGUI ScoreText;
        [SerializeField] private int currentScore;

        [Header("Currency Anim")]
        [SerializeField] private RectTransform currencyTrans;
        [SerializeField] private TextMeshProUGUI CurrencyText;
        [SerializeField] private int currentCurrency;

        [Header("Buttons Anim")]
        [SerializeField] private RectTransform buttonsRectTrans;

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
            GameOverGO.SetActive(false);
           // PanelIntro(); // for testing
        }

        #region tween anim

        private void PanelIntro()
        {
            GameOverGO.SetActive(true);
            panelTitleText.maxVisibleCharacters = 0;
            ScoreTitleText.maxVisibleCharacters = 0;

            Time.timeScale = 0;
            Sequence introTween = DOTween.Sequence();

            introTween.Append(canvasGroup.DOFade(1, tweenDuration).SetUpdate(true))
                      .Append(bgRectTransform.DOLocalMoveY(908, tweenDuration).SetUpdate(true).SetEase(easeIn))
                      .Append(panelTitleHolder.DOLocalMoveY(1181, tweenDuration).SetUpdate(true).SetEase(easeIn))
                      .Append(TypewriterTween(panelTitleText))  
                      .Append(finalScorePanel.DOLocalMoveY(1063, tweenDuration).SetUpdate(true).SetEase(easeIn))
                      .Append(TypewriterTween(ScoreTitleText))  
                      .Append(ScoreNumberTextCanvasGrp.DOFade(1, tweenDuration).SetUpdate(true))
                      .Append(currencyTrans.DOLocalMoveY(720, tweenDuration).SetUpdate(true).SetEase(easeIn))
                      .Append(buttonsRectTrans.DOLocalMoveY(550, tweenDuration).SetUpdate(true).SetEase(easeIn))
                      .OnComplete(() => {
                          ScoreDisplay();
                          CurrencyDisplay();
                      }).SetUpdate(true);
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
                ScoreText.text = x.ToString("D5");
            }).SetUpdate(true);
        }

        private void CurrencyDisplay()
        {
            DOVirtual.Int(currentCurrency, currencyData.matchCoins, 1, (y) =>
            {
                CurrencyText.text = y.ToString("D5"); 
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
    }
}
