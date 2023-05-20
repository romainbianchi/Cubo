using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBehaviour : MonoBehaviour
{
    // Keep track of the initial parent
    private Transform initialParent;


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
        transform.SetParent(initialParent);

        // Keep the same position
        
    }
}
