using AudioScripts.AudioSettings;
using EventScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using AudioType = AudioScripts.AudioSettings.AudioType;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;

public class PauseManager : MonoBehaviour
{
    private static PauseManager _instance;
    public static PauseManager Instance => _instance;

    private bool _isPauseScreenOpen;
    private bool _isTutorialScreenOpen;

    [Header("BGM Audio")]
    [SerializeField] private Sprite _bgmOn;
    [SerializeField] private Sprite _bgmOff;
    [SerializeField] private UnityEngine.UI.Button _bgmButton;

    [Header("SFX Audio")]
    [SerializeField] private Sprite _sfxOn;
    [SerializeField] private Sprite _sfxOff;
    [SerializeField] private UnityEngine.UI.Button _sfxButton;

    [Header("Tween Animation")]
    [SerializeField] private GameObject pausePanelGO;
    [SerializeField] private CanvasGroup pausefadePanel;
    [SerializeField] private float pauseTopY;
    [SerializeField] private float pauseMidY;
    [SerializeField, Range(0, 1)] private float tweenDuration;

    [SerializeField] private GameObject tutorialPanelGO;
    [SerializeField] private CanvasGroup tutorialfadePanel;
    [SerializeField] private float tutorialTopY;
    [SerializeField] private float tutorialMidY;

    [SerializeField] private CanvasGroup loadSceneFadePanel;

    private bool _isBgmOn;
    private bool _isSfxOn;
    private RectTransform pauseRectTrans;
    private RectTransform tutorialRectTrans;
    public AudioUIManager AudioUIManager;

    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this);

        GameEvents.ON_PAUSE += TogglePause;

        GameEvents.TRIGGER_TUTORIAL += ToggleTutorial;

        if (pausePanelGO != null)
            pauseRectTrans = pausePanelGO.GetComponent<RectTransform>();

        if (tutorialPanelGO != null)
            tutorialRectTrans = tutorialPanelGO.GetComponent<RectTransform>();
    }

    private void OnDestroy()
    {
        GameEvents.ON_PAUSE -= TogglePause;
        GameEvents.TRIGGER_TUTORIAL -= ToggleTutorial;
    }

    private async void Start()
    {
        _isPauseScreenOpen = false;
        _isTutorialScreenOpen = false;

        await FadeOutPanel();
    }

    #region Pause Screen

    public async void TogglePause()
    {
        if(!_isPauseScreenOpen && !_isTutorialScreenOpen)
        {
            _isPauseScreenOpen = true;
            Time.timeScale = 0;
            AudioEvents.ON_STOP_SFX?.Invoke();
            pausefadePanel.interactable = true;
            pausefadePanel.blocksRaycasts = true;
            PausePanelIntro();
        }
        else
        {
            await PausePanelOutro();
            _isPauseScreenOpen = false;
            pausefadePanel.interactable = false;
            pausefadePanel.blocksRaycasts = false;
            Time.timeScale = 1;
        }
    }

    private void PausePanelIntro()
    {
        pausefadePanel.DOFade(1, tweenDuration).SetUpdate(true);
        pauseRectTrans.DOAnchorPosY(pauseMidY, tweenDuration).SetUpdate(true);
    }

    async Task PausePanelOutro()
    {
        pausefadePanel.DOFade(0, tweenDuration).SetUpdate(true);
        await pauseRectTrans.DOAnchorPosY(pauseTopY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
    #endregion

    #region Tutorial Screen

    public async void ToggleTutorial()
    {
        if (!_isTutorialScreenOpen && !_isPauseScreenOpen)
        {
            _isTutorialScreenOpen = true;
            Time.timeScale = 0;
            tutorialfadePanel.interactable = true;
            tutorialfadePanel.blocksRaycasts = true;
            TutorialPanelIntro();
        }
        else
        {
            await TutorialPanelOutro();
            _isTutorialScreenOpen = false;
            tutorialfadePanel.interactable = false;
            tutorialfadePanel.blocksRaycasts = false;
            Time.timeScale = 1;
        }
    }

    private void TutorialPanelIntro()
    {
        tutorialfadePanel.DOFade(1, tweenDuration).SetUpdate(true);
        tutorialRectTrans.DOAnchorPosY(tutorialMidY, tweenDuration).SetUpdate(true);
    }

    async Task TutorialPanelOutro()
    {
        tutorialfadePanel.DOFade(0, tweenDuration).SetUpdate(true);
        await tutorialRectTrans.DOAnchorPosY(tutorialTopY, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
    #endregion


    #region buttons

    public void Resume()
    {
        TogglePause();
    }

    public void MuteBGM()
    {
        _isBgmOn = !_isBgmOn;
        _bgmButton.image.sprite = _isBgmOn ? _bgmOn : _bgmOff;
        AudioManager.Instance.AudioMute(!_isBgmOn, AudioType.BGM);

        if (!_isBgmOn) { AudioUIManager.bgmSlider.interactable = false; }
        else { AudioUIManager.bgmSlider.interactable = true; }
    }

    public void MuteSFX()
    {
        _isSfxOn = !_isSfxOn;
        _sfxButton.image.sprite = _isSfxOn ? _sfxOn : _sfxOff;
        AudioManager.Instance.AudioMute(!_isSfxOn, AudioType.Sfx);
        
        if(!_isSfxOn) {AudioUIManager.sfxSlider.interactable = false; }
        else { AudioUIManager.sfxSlider.interactable = true; }

    }

    public void TogglePauseButton()
    {
        GameEvents.ON_PAUSE?.Invoke();
    }

    public void ToggleTutorialButton()
    {
        GameEvents.TRIGGER_TUTORIAL?.Invoke();
    }

    public void MainMenuButton(int sceneInt)
    {
        StartCoroutine(GoToScene(sceneInt));
    }

    IEnumerator GoToScene(int sceneInt)
    {
        if(_isPauseScreenOpen)
        {
            TogglePause();
        }
        else if (_isTutorialScreenOpen)
        {
            ToggleTutorial();
        }

        loadSceneFadePanel.DOFade(1, 0.2f).SetUpdate(true);
        yield return new WaitForSeconds(0.2f);

        SceneController.Instance.LoadScene(sceneInt);
    }

    async Task FadeOutPanel()
    {
        await loadSceneFadePanel.DOFade(0, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
    #endregion
}


