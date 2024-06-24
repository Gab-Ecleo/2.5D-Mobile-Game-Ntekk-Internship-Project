using System.Collections;
using System.Collections.Generic;
using ScriptableData;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    //[SerializeField] private GameObject Spring;
    [SerializeField] private float _powerDuration = 3f;
    [SerializeField] private PlayerStatsSO currentPlayerStats;
    [SerializeField] private float _currentJumpHeight;
    [SerializeField] private float _jumpBoost = 3;
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
        yield return new WaitForSeconds(_powerDuration);
        _currentJumpHeight -= _jumpBoost;
        currentPlayerStats.jumpHeight = _currentJumpHeight;
        Debug.Log("Spring Off");
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
