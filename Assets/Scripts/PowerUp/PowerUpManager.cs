using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    //[SerializeField] private GameObject Spring;
    [Header("Power Up Duration")]
    [SerializeField] private float _powerDuration = 3f;
    [Header("Jump Stat")]
    [SerializeField] private PlayerStatsSO currentPlayerStats;
    [SerializeField] private float _currentJumpHeight;
    [SerializeField] private float _jumpBoost = 3;

    [Header("Slow Motion Stat")] 
    [SerializeField] private float _slowMotionFactor = 0.1f;
    [SerializeField] private float _originalTimeScale;
    [Header("Power Up Decay Duration")]
    [SerializeField] private float _decayDuration = 8f;
    
    
    private void Start()
    {
        _currentJumpHeight = currentPlayerStats.jumpHeight;
    }
    
    public IEnumerator SpringPower()
    {
        _currentJumpHeight += _jumpBoost;
        currentPlayerStats.jumpHeight = _currentJumpHeight;
        Debug.Log("Spring On");
        yield return new WaitForSecondsRealtime(_powerDuration);
        _currentJumpHeight -= _jumpBoost;
        currentPlayerStats.jumpHeight = _currentJumpHeight;
        Debug.Log("Spring Off");
        Destroy(GameObject.FindWithTag("PowerUp"));
    }

    public IEnumerator SlowMoPower()
    {
        _originalTimeScale = Time.timeScale;
        Time.timeScale = _slowMotionFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.01f;
        Debug.Log("Super Hot");
        yield return new WaitForSecondsRealtime(_powerDuration);
        Time.timeScale = _originalTimeScale;
        Debug.Log("Super Cold");
        Destroy(GameObject.FindWithTag("PowerUp"));
    }

    public IEnumerator Decay()
    {
        yield return new WaitForSeconds(_decayDuration);
        _currentJumpHeight -= _jumpBoost;
        currentPlayerStats.jumpHeight = _currentJumpHeight;
        Debug.Log("Power Up Decayed");
        Destroy(GameObject.FindWithTag("PowerUp"));
    }
}
