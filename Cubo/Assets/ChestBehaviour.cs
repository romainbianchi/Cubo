using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehaviour : MonoBehaviour
{

    //Chest roofs
    public GameObject roofOpen;
    public GameObject roofClosed;

    // Start is called before the first frame update
    void Start()
    {
        roofOpen.SetActive(false);
        roofClosed.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.name == "Key")
        {
            roofOpen.SetActive(true);
            roofClosed.SetActive(false);
            other.gameObject.SetActive(false);
        }
    }

    public bool isOpen()
    {
        return roofOpen.activeSelf;
    }
}
