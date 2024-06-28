using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMo : MonoBehaviour
{
    private PowerUpManager _powerUp;
    void Start()
    {
        _powerUp = GameObject.FindWithTag("PowerUp Manager").GetComponent<PowerUpManager>();
        StartCoroutine(_powerUp.Decay());
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(_powerUp.SlowMoPower());
        GetComponent<Collider>().enabled = false;
        GameObject.Find("Barrier").GetComponent<Collider>().enabled = false;
        GameObject.Find("Barrier").GetComponent<MeshRenderer>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }
}
