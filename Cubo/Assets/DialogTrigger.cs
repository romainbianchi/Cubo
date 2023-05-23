using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public OVRPlayerController playerController;
    private Collider collider;

    private bool inArea = false;

    // Start is called before the first frame update
    void Start()
    {
        collider = playerController.GetComponent<CharacterController>().GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == collider)
        {
            inArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == collider)
        {
            inArea = false;
        }
    }

    // get state
    public bool isInArea()
    {
        return inArea;
    }
}
