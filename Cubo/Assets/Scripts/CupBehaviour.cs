using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupBehaviour : MonoBehaviour
{
    // Keep track of all the objects in the cup
    private List<GameObject> objectsInCup = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject iceCube in objectsInCup)
        {

            // if distance between cup and ice cube is greater than 0.1
            if (Vector3.Distance(iceCube.transform.position, transform.position) > 0.3f)
            {
                // remove ice cube from cup
                objectsInCup.Remove(iceCube);
                // set initial parent
                iceCube.GetComponent<IceBehaviour>().setToInitialParent();
            }

        }
    }

    // If an object enters the trigger
    private void OnTriggerStay(Collider other)
    {

        // If the cup is upside down, do nothing
        if (isUpsideDown()) return;

        if (fullCup()) return;
        
        if (objectsInCup.Contains(other.gameObject)) return;

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
        iceCube.transform.localPosition = new Vector3(0, 0.04f, 0);
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
        iceCube.transform.localPosition = new Vector3(0, 0.04f, 0);
    }

    

    private bool isUpsideDown()
    {
        // Get the cup up vector
        Vector3 cupUpVector = transform.up;
        
        // If the cup is upside down
        if (cupUpVector.y < 0) return true;
        return false;
        
    }

    private bool fullCup(){
        // if cup is full return true
        if (objectsInCup.Count > 3) return true;
        return false;
    }

}