using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PressureEffect : MonoBehaviour
{
    [SerializeField] private float hazardDuration = 5f;

    [SerializeField] private Rigidbody playerRb;
    
    [SerializeField] private float windStr = 200f;
    [SerializeField] private Vector3 windDir;
    
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
            StartCoroutine(nameof(WindOn));
        }
    }
    
    void BlowWind()
    {
        Debug.Log("Pushing player :<");
        playerRb.AddForce(windDir * windStr * Time.deltaTime, ForceMode.VelocityChange);
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
        windDir.x = PickDirection();
        
        yield return new WaitForSeconds(hazardDuration);
        Debug.Log("Stopping Wind force");
        _isCoRunning = false;
    }
}
