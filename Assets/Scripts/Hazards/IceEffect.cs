using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEffect : MonoBehaviour
{
    [SerializeField] private PhysicMaterial _slipperyMaterial;
    [SerializeField] private PhysicMaterial _defaultMaterial;
    [SerializeField] private Collider _platformColl;
    [SerializeField] private Collider _blockColl;
    
    [SerializeField] private float _hazardDuration = 5f;
    
    
    private void Update()
    {
        // CHANGE TRIGGER in the Future!!! Activates Hazard when button is pressed
        if (Input.GetKeyDown("u"))
        {
            StartCoroutine("slideOn");
        }
    }
    
    //Changes the material of the objects to a slippery ice material for [Hazard Duration], then returns to the original material
    IEnumerator slideOn()
    {
        _platformColl.material = _slipperyMaterial;
        _blockColl.material = _slipperyMaterial;
        Debug.Log("Slide them platforms :<");
        yield return new WaitForSeconds(_hazardDuration);
        _platformColl.material = _defaultMaterial;
        _blockColl.material = _defaultMaterial;
        Debug.Log("Returning Platform material :>");
    }
}
