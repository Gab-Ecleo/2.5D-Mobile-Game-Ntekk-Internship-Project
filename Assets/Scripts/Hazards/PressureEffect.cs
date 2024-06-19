using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PressureEffect : MonoBehaviour
{
    [SerializeField] private float _hazardDuration = 5f;

    [SerializeField] private Rigidbody _playerRB;
    
    [SerializeField] private float _windStr = 200f;
    [SerializeField] private Vector3 _windDir;
    
    public Vector3 randomVal;
    private bool _isCoRunning;

    private void FixedUpdate()
    {
        if (_isCoRunning)
            BlowWind();
    }

    private void Update()
    {
        if (Input.GetKeyDown("j") && !_isCoRunning)
        {
            StartCoroutine("WindOn");
        }
    }
    
    void BlowWind()
    {
        Debug.Log("Pushing player :<");
        _playerRB.AddForce(_windDir * _windStr * Time.deltaTime, ForceMode.VelocityChange);
    }

    private int PickDirection()
    {
        int x = Random.Range(-1, 2);

        if (x == 0)
            x = 1;

        return x;
    }
    
    //This will serve has the hazard's timer
    private IEnumerator WindOn()
    {
        _isCoRunning = true;
        
        //randomize wind direction
        _windDir.x = PickDirection();
        
        yield return new WaitForSeconds(_hazardDuration);
        Debug.Log("Stopping Wind force");
        _isCoRunning = false;
    }
}
