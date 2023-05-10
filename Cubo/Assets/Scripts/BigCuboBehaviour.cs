using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BigCuboBehaviour : MonoBehaviour
{
    public GameObject smallCubo;

    void Update()
    {
        transform.eulerAngles = smallCubo.transform.eulerAngles;
    }
}