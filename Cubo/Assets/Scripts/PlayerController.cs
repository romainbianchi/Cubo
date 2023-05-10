using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Refer to a position of in_cubo
    public GameObject inCuboPos;

    private static bool inCubo = false;
    private static Vector3 deskPos;

    public void TpPlayer()
    {
        // if the player is in the cube, teleport them to the center
        if (!inCubo)
        {
            transform.position = inCuboPos.transform.position;
            inCubo = true; 
            
        } else {
            inCuboPos.transform.position = transform.position;
            transform.position = deskPos;
            inCubo = false;
        }
    }


	void Start () 
    { 
        // Set the initial position of the desk pos
        deskPos = transform.position;

        // Set the initial position of the in_cubo_pos
        inCuboPos.transform.position = new Vector3(inCuboPos.transform.position.x, transform.position.y, inCuboPos.transform.position.z);
    }

    void Update()
    {
        
    } 

}