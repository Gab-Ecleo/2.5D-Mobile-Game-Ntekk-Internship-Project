using System;
using System.Collections;
using System.Collections.Generic;
using PlayerScripts;
using ScriptableData;
using Unity.Mathematics;
using UnityEngine;

public class PressureEffect : MonoBehaviour
{
    [SerializeField] private float _hazardDuration = 5f;
    [SerializeField] private PlayerStatsSO currentPlayerStats;
    [SerializeField] private float force;
    [SerializeField] private Rigidbody _playerRB;
    [SerializeField] private GameObject _player;
    public LayerMask m_LayerMask;
    
    private void Update()
    {
        // CHANGE TRIGGER in the Future!!! Activates Hazard when button is pressed
        if (Input.GetKeyDown("j"))
        {
            StartCoroutine("windOn");
        }
    }

    private void windBlow()
    {
        _playerRB.constraints = RigidbodyConstraints.FreezeAll;
        Collider[] objects =
            Physics.OverlapBox(_player.transform.position, transform.localScale / 2, quaternion.identity, m_LayerMask);
        foreach (var obj in objects)
        {
            Vector2 direction = obj.transform.position - _player.transform.position;
            obj.GetComponent<Rigidbody>().AddForce(direction * force);
        }
    }

    //Halves the player's speed for [Hazard Duration], then returns to the original value
    private IEnumerator windOn()
    {
        windBlow();
        Debug.Log("Pushing player :<");
        yield return new WaitForSeconds(_hazardDuration);
        
        Debug.Log("Returning :>");
    }
}
