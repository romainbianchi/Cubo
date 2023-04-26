using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeRotation : MonoBehaviour
{
    public GameObject ReferenceCube;

    void Update()
    {
        transform.eulerAngles = ReferenceCube.transform.eulerAngles;
    }
}