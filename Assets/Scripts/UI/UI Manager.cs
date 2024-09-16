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
using Player_Statistics;
using ScriptableData;
using TMPro;
using UnityEngine.InputSystem.OnScreen;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    [Header("Check Scene")]
    [SerializeField] private bool isInMainMenu;

    [Header("Barrier")]
    [SerializeField] TMP_Text barrierText;

    [Header("BGM Audio")]
    [SerializeField] private Sprite _bgmOn;
    [SerializeField] private Sprite _bgmOff;
    [SerializeField] private UnityEngine.UI.Button _bgmButton;

    [Header("SFX Audio")]
    [SerializeField] private Sprite _sfxOn;
    [SerializeField] private Sprite _sfxOff;
    [SerializeField] private UnityEngine.UI.Button _sfxButton;

    [Header("Pause Tween Animation")]
    [SerializeField] private GameObject pausePanelGO;
    [SerializeField] private CanvasGroup pausefadePanel;
    [SerializeField] private float pauseTopY;
    [SerializeField] private float pauseMidY;
    [SerializeField, Range(0, 1)] private float tweenDuration;

    [Header("Tutorial Tween Animation")]
    [SerializeField] private GameObject tutorialPanelGO;
    [SerializeField] private CanvasGroup tutorialfadePanel;
    [SerializeField] private float tutorialTopY;
    [SerializeField] private float tutorialMidY;

    [SerializeField] private CanvasGroup SceneFadePanel;

    [Header("Ref")]
    public Button TutorialButton;
    public GameObject PauseMenu;
    public GameObject TutorialMenu;
    public AudioUIManager AudioUIManager;
    public SwipeController[] SwipeController;

    [Header("Panels")]
    [SerializeField] private Image MovementPanel;

    private bool _isBgmOn;
    private bool _isSfxOn;
    private RectTransform pauseRectTrans;
    private RectTransform tutorialRectTrans;
    private PlayerStatsSO initialStat;
    private PlayerStatsSO currStat;
    private bool _isPauseScreenOpen;
    private bool _isTutorialScreenOpen;
    private void Awake()
    {
        if (_instance == null) _instance = this;
        else Destroy(this);

        GameEvents.ON_PAUSE += TogglePause;

        GameEvents.ON_TUTORIAL += ToggleTutorial;

        PlayerEvents.ON_BARRIER_HIT += BarrierUpdate;

        if (pausePanelGO != null)
            pauseRectTrans = pausePanelGO.GetComponent<RectTransform>();

        if (tutorialPanelGO != null)
            tutorialRectTrans = tutorialPanelGO.GetComponent<RectTransform>();

        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }



    }

    private void OnDestroy()
    {
        GameEvents.ON_PAUSE -= TogglePause;
        GameEvents.ON_TUTORIAL -= ToggleTutorial;
        PlayerEvents.ON_BARRIER_HIT -= BarrierUpdate;
    }

    private async void Start()
    {
        InitializePlayerStats();
        InitializeUIStates();

        await FadeOutPanel();

        FirstTutorial();

        barrierText.text = currStat.stats.barrierDurability.ToString();

        if (MovementPanel != null)
        {
            MovementPanel.enabled = false;
        }
    }

    private void InitializePlayerStats()
    {
        initialStat = GameManager.Instance.FetchInitialPlayerStat();
        currStat = GameManager.Instance.FetchCurrentPlayerStat();
    }

    private bool InitializeUIStates()
    {
        _isPauseScreenOpen = false;
        _isTutorialScreenOpen = false;

        PauseMenu.SetActive(_isPauseScreenOpen);
        TutorialMenu.SetActive(_isTutorialScreenOpen);

        return !_isPauseScreenOpen && !_isTutorialScreenOpen && !PauseMenu.activeSelf && !TutorialMenu.activeSelf;
    }

    // open tutorial when its player first time 
    private void FirstTutorial()
    {
        if (initialStat.stats.isPlayerFirstGame && !isInMainMenu)
        {
            Debug.Log("First Time Player");
            GameEvents.ON_TUTORIAL?.Invoke();
        }
        else if (initialStat.stats.isPlayerFirstGame && isInMainMenu)
        {
            TutorialButton.interactable = false;
        }
        else if (!initialStat.stats.isPlayerFirstGame && isInMainMenu)
        {
            TutorialButton.interactable = true;
        }
    }

    private void BarrierUpdate()
    {
        barrierText.text = currStat.stats.barrierDurability.ToString("d4");
    }

    #region Pause Screen

    private async void TogglePause()
    {
        PauseMenu.SetActive(true);
        if (!_isPauseScreenOpen && !_isTutorialScreenOpen)
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
            PauseMenu.SetActive(false);
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

    private async void ToggleTutorial()
    {
        TutorialMenu.SetActive(true);
        if (!_isTutorialScreenOpen && !_isPauseScreenOpen )
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

            foreach (var item in SwipeController)
            {
                item.ResetContent();
            }

            _isTutorialScreenOpen = false;
            tutorialfadePanel.interactable = false;
            tutorialfadePanel.blocksRaycasts = false;
            Time.timeScale = 1;
            TutorialMenu.SetActive(false);
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


    #region Buttons

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

        if (!_isSfxOn) { AudioUIManager.sfxSlider.interactable = false; }
        else { AudioUIManager.sfxSlider.interactable = true; }

    }

    public void TogglePauseButton()
    {
        GameEvents.ON_PAUSE?.Invoke();
    }

    public void ToggleTutorialButton()
    {
        GameEvents.ON_TUTORIAL?.Invoke();
    }

    public void ToggleScreenControlButton()
    {
        GameEvents.ON_CONTROLS?.Invoke();
    }

    public void GoToScene(int scene)
    {
        StartCoroutine(SceneTransition(scene, true));
    }

    public void GoToHomeButton()
    {
        GameEvents.TRIGGER_END_OF_GAMEEND_SCREEN.Invoke();
        StartCoroutine(SceneTransition(0, true));
    }

    public void GoToUpgradeButton()
    {
        GameEvents.TRIGGER_END_OF_GAMEEND_SCREEN.Invoke();
        StartCoroutine(SceneTransition(0, false));
    }

    IEnumerator SceneTransition(int sceneInt, bool isDefaultHome)
    {
        if (_isPauseScreenOpen)
        {
            TogglePause();
        }
        else if (_isTutorialScreenOpen)
        {
            ToggleTutorial();
        }

        SceneFadePanel.DOFade(1, 0.2f).SetUpdate(true);
        yield return new WaitForSeconds(0.25f);

        DOTween.KillAll();
        SceneController.Instance.LoadScene(sceneInt, isDefaultHome);
    }

    async Task FadeOutPanel()
    {
        await SceneFadePanel.DOFade(0, tweenDuration).SetUpdate(true).AsyncWaitForCompletion();
    }
    #endregion
}
