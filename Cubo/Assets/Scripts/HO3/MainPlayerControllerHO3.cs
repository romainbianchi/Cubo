using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerControllerHO3 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(OVRInput.Get(OVRInput.Button.One)) {
            Debug.LogWarning( "Button is pressed" );
        }
    }
}
