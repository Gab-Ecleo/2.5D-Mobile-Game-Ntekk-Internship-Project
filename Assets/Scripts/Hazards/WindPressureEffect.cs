using System.Collections;
using AudioScripts;
using EventScripts;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public enum WindDir
{
    Left,
    Right
}

public class WindPressureEffect : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float _hazardDuration = 5f;
    [SerializeField] private float _windStrength = 200f;
    [SerializeField] private WindDir _windDirection;
    [SerializeField] public GameObject _windParticlesL;
    [SerializeField] public GameObject _windParticlesR;
    
    [Header("Dependencies")]
    [SerializeField] private Rigidbody _playerRB;

    private Vector3 _dir;
    private bool _isHazardActive;
    private GameObject _windParticles;
    private GameManager _gameManager;

    #region UNITY METHODS

    private void Awake()
    {
        GameEvents.TRIGGER_WIND_HAZARD += TriggerWindHazard;
    }

    private void OnDestroy()
    {
        GameEvents.TRIGGER_WIND_HAZARD -= TriggerWindHazard;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _isHazardActive = false;
    }

    private void FixedUpdate()
    {
        if (_gameManager.IsGameOver()) return;
        if (_gameManager.FetchGameStateData().isPaused) return;

        if (_isHazardActive)
            BlowWind();
    }
    
    #endregion
    
    // public for debugging
    public void TriggerWindHazard()
    {
        if (_isHazardActive || _gameManager.IsGameOver()) return;

        StartCoroutine(EnableWind());
    }
    
    //Start Hazard timer
    private IEnumerator EnableWind()
    {
        SetBoolCondition(true);
        
        // Plays SFX correlating to the action
        AudioEvents.ON_HAZARD_TRIGGER?.Invoke("wind");
        
        //randomize wind direction
        _dir.x = PickDirection();
        SetWindDirection();
        
        yield return new WaitForSeconds(_hazardDuration);
        
        SfxScript.Instance.StopSFX();
        
        _windParticles.SetActive(false);
        SetBoolCondition(false);
    }
    
    private void SetBoolCondition(bool condition)
    {
        _gameManager.FetchHazardData().IsWindActive = condition;
        _isHazardActive = condition;
    }

    private void SetWindDirection()
    {
        switch (_dir.x)
        {
            case 1:
                _windDirection = WindDir.Right; 
                _windParticles = Instantiate(_windParticlesL);
                break;
            
            case -1:
                _windDirection = WindDir.Left;
                _windParticles = Instantiate(_windParticlesR);
                break;
        }
    }
    
    #region MATHEMTICAL FUNCTIONS IGNORE THIS SECTION

    void BlowWind()
    {
        Vector3 windForce = _windStrength * _dir;
        
        _playerRB.AddForce(windForce * Time.unscaledTime);
    }
    
    private int PickDirection()
    {
        int x = Random.Range(-1, 2);

        if (x == 0)
            x = 1;
        
        return x;
    }

    #endregion
    
}
