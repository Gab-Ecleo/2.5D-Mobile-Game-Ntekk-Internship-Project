using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IceEffect : MonoBehaviour
{
    [SerializeField] private PhysicMaterial slipperyMaterial;
    [SerializeField] private PhysicMaterial defaultMaterial;
    [SerializeField] private Collider platformColl;
    [SerializeField] private Collider blockColl;
    
    [SerializeField] private float hazardDuration = 5f;
    
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
        platformColl.material = slipperyMaterial;
        blockColl.material = slipperyMaterial;
        Debug.Log("Slide them platforms :<");
        yield return new WaitForSeconds(hazardDuration);
        platformColl.material = defaultMaterial;
        blockColl.material = defaultMaterial;
        Debug.Log("Returning Platform material :>");
    }
}
