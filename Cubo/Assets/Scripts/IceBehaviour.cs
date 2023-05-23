using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBehaviour : MonoBehaviour
{
    // Keep track of the initial parent
    private Transform initialParent;

    // Initial position
    private Vector3 initialPosition;
    
    

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial parent
        initialParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {   
    }

    public void setToInitialParent()
    {   
        if (transform.position.x < 10.0f) transform.SetParent(null);
        else transform.SetParent(initialParent);

    }

    public void Respawn(){
        transform.position = initialPosition;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.SetParent(initialParent);
    }
}
