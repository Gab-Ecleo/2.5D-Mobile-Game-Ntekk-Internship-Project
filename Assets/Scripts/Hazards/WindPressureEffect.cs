using System;
using System.Collections;
using AudioScripts;
using AudioScripts.AudioSettings;
using EventScripts;
using UnityEngine;
using UnityEngine.Serialization;
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
    private bool _isCorActive;
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
        _isCorActive = false;
    }

    private void FixedUpdate()
    {
        if (_gameManager.IsGameOver()) return;
        
        if (_isCorActive)
            BlowWind();
    }
    
    #endregion
    
    private void TriggerWindHazard()
    {
        if (_isCorActive || _gameManager.IsGameOver()) return;

        StartCoroutine(EnableWind());
        // Plays SFX correlating to the action
    }
    
    //Start Hazard timer
    private IEnumerator EnableWind()
    {
        _gameManager.FetchHazardData().IsWindActive = true;
        _isCorActive = true;
        
        AudioEvents.ON_HAZARD_TRIGGER?.Invoke("wind");
        
        //randomize wind direction
        _dir.x = PickDirection();
        
        //Display Wind Direction in Inspector (For testing only, can be removed if needed)
        SetLabel();
        
        yield return new WaitForSeconds(_hazardDuration);
        Debug.Log("End of Hazard Duration");
        SfxScript.Instance.StopSFX();
        _windParticles.SetActive(false);
        _isCorActive = false;
        _gameManager.FetchHazardData().IsWindActive = false;
    }

    private void SetLabel()
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
        
        _playerRB.AddForce(windForce * Time.deltaTime, ForceMode.VelocityChange);
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
