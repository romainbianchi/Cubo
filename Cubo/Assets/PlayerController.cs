using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Refer to a position of an object insteand of using (15,2,0)
    public GameObject platform;


    private static bool inCube = false;
    private static Vector3 oldPos;

	void Start () 
    { 

    }

    void Update()
    {
        // if ovrinput button one is pressed, tp the player to (15,2,0)
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            // if the player is in the cube, teleport them to the center
            if (!inCube)
            {
                oldPos = transform.position;
                transform.position = platform.transform.position;
                inCube = true; 
                
            } else {
                transform.position = oldPos;
                inCube = false;
                
            }
        }
        
    }

}