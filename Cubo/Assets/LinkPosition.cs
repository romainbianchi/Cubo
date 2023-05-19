using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPosition : MonoBehaviour
{
    public OVRCameraRig cameraRig;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = cameraRig.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraRig.transform.position;
    }
}
