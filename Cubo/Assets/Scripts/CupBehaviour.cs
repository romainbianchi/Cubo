using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupBehaviour : MonoBehaviour
{
    // Keep track of all the objects in the cup
    private List<GameObject> objectsInCup = new List<GameObject>();

    // Full of water
    private bool water = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpsideDown()) {
            // parent
            foreach (GameObject iceCube in objectsInCup)
            {
                iceCube.GetComponent<IceBehaviour>().setToInitialParent();
                iceCube.GetComponent<Rigidbody>().isKinematic = false;
            }
            
            objectsInCup.Clear();
        } else foreach (GameObject iceCube in objectsInCup)
        {
            // Bound their position to the cup
            iceCube.transform.position = transform.position + transform.up * 0.02f * ((objectsInCup.IndexOf(iceCube))-2) + transform.right * 0.01f * ((objectsInCup.IndexOf(iceCube))%2);

            // Bound their rotation to the cup
            iceCube.transform.rotation = transform.rotation;
        }

        Debug.Log(objectsInCup.Count);
    }

    // If an object enters the trigger
    private void OnTriggerStay(Collider other)
    {

        // If the cup is upside down, do nothing
        if (isUpsideDown()) return;

        if (fullCup() || water) return;

        if (objectsInCup.Contains(other.gameObject)) return;

        // If the object has the "ice behaviour" script
        if (other.gameObject.GetComponent<IceBehaviour>() == null) return;
        GameObject iceCube = other.gameObject;
        
        // and the object is not grabbed by the player
        if (!iceCube.GetComponent<ObjectGrabbable>().IsAvailable()) return;
        
        // Add the object to the cup
        objectsInCup.Add(iceCube);

        // set parent
        iceCube.transform.parent = transform;

        // Set kinematic
        iceCube.GetComponent<Rigidbody>().isKinematic = true;
    }

    public bool isUpsideDown()
    {
        return transform.up.y < 0;
    }

    public bool fullCup(){
        return objectsInCup.Count >= 3 ;
    }

    public void meltIce()
    {
        if (water) return;
        if (objectsInCup.Count < 3) return;

        water = true;
        GetComponent<MeshRenderer>().enabled = true;

        foreach (GameObject iceCube in objectsInCup)
        {
            iceCube.GetComponent<IceBehaviour>().Respawn();
        }
        objectsInCup.Clear();
    }

    public bool isWater()
    {
        return water;
    }

    public void pourWater()
    {
        water = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

}