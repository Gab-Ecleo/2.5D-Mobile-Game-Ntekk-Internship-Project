using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffect : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private float _hazardDuration = 5f;
    
    [Header("Dependencies")]
    [SerializeField] private PhysicMaterial _slipperyMaterial;
    [SerializeField] private PhysicMaterial _defaultMaterial;
    [SerializeField] private Collider _platformColl;
    [SerializeField] private Collider _blockColl;

    private bool _isCorActive;
    private GameManager _gameManager;
    
    #region UNITY METHODS

    private void Awake()
    {
        GameEvents.TRIGGER_ICE_HAZARD += TriggerIceHazard;
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _isCorActive = false;
    }

    private void OnDestroy()
    {
        GameEvents.TRIGGER_ICE_HAZARD -= TriggerIceHazard;
    }

    #endregion
    
    private void TriggerIceHazard()
    {
        if (_isCorActive || _gameManager.IsGameOver()) return;
        
        StartCoroutine(ApplyIcePlatform());
    }
    
    //Changes the material of the objects to a slippery ice material for [Hazard Duration], then returns to the original material
    IEnumerator ApplyIcePlatform()
    {
        _isCorActive = true;
        Debug.Log("Applying Ice Hazard");
        
        _platformColl.material = _slipperyMaterial;
        _blockColl.material = _slipperyMaterial;
        
        yield return new WaitForSeconds(_hazardDuration);
        
        Debug.Log("End of Hazard Duration");
        
        _platformColl.material = _defaultMaterial;
        _blockColl.material = _defaultMaterial;
        _isCorActive = false;
    }
}
