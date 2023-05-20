using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupBehaviour : MonoBehaviour
{
    // Keep track of all the objects in the cup
    private List<GameObject> objectsInCup = new List<GameObject>();

    // Materials
    public Material red;
    public Material green;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // change the color of the cup if it is upside down
        if (isUpsideDown()) {
            // Color red
            GetComponent<Renderer>().material = red;
        } else {
            // Color green
            GetComponent<Renderer>().material = green;
        }

    }

    // If an object enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        // If the cup is upside down, do nothing
        if (isUpsideDown()) return;

        // If the object has the "ice behaviour" script
        if (other.gameObject.GetComponent<IceBehaviour>() == null) return;
        GameObject iceCube = other.gameObject;
        
        // and the object is not grabbed by the player
        if (!iceCube.GetComponent<ObjectGrabbable>().IsAvailable()) return;
        
        // Add the object to the cup
        objectsInCup.Add(iceCube);

        // Set the cup as the parent of the object
        iceCube.transform.parent = transform;

        // Set the local position of the object to 0
        iceCube.transform.localPosition = new Vector3(0, 0.1f, 0);
    }

    void OnTriggerExit(Collider other)
    {
        // If the object is in the list 
        if (!objectsInCup.Contains(other.gameObject)) return;
        GameObject iceCube = other.gameObject;

        // If the cup is upside down, do nothing
        if (isUpsideDown()) {
            objectsInCup.Remove(iceCube);
            iceCube.GetComponent<IceBehaviour>().setToInitialParent();

            // set speed to 0
            iceCube.GetComponent<Rigidbody>().velocity = Vector3.zero;

            return;
        }

        // and the object is not manually put out of the cup
        if (!iceCube.GetComponent<ObjectGrabbable>().IsAvailable()) {
            objectsInCup.Remove(iceCube);
            // iceCube.GetComponent<IceBehaviour>().setToInitialParent();
            return;
        }

        // put back the object in the cup (local position to 0)
        iceCube.transform.localPosition = new Vector3(0, 0.1f, 0);
    }

    

    private bool isUpsideDown()
    {
        // Get the cup up vector
        Vector3 cupUpVector = transform.up;
        
        // If the cup is upside down
        if (cupUpVector.y < 0) return true;
        return false;
        
    }


}
