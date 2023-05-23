using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehaviour : MonoBehaviour
{

    // All children stones
    private GameObject[] stones;

    // Minimum velocity to hit the wall
    public float minHitVelocity = 0.5f;

    private bool destroyed = false;

    public PlayerControllerPers playerPers;

    public GameObject backCollider;

    // Start is called before the first frame update
    void Start()
    {
        // Get the childrens
        stones = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            stones[i] = transform.GetChild(i).gameObject;
        } 

        // Set all the stones rigidbody to kinematic
        foreach (GameObject stone in stones)
        {
            stone.GetComponent<Rigidbody>().isKinematic = true;
        }

        // debug print the name of the stones
        foreach (GameObject stone in stones)
        {
            Debug.Log(stone.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other) {

        if(other.GetComponent<ObjectGrabbable>().importantObject != ObjectGrabbable.ImportantObject.Stick) return;
        
        // left hand is grabbing the stick
        if (playerPers.getLeftState() == PlayerControllerPers.State.Grabbing || playerPers.getLeftState() == PlayerControllerPers.State.DistanceGrabbing) {
            // get magnitude of velocity of controllers
            if (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch).magnitude < minHitVelocity) {
                return;
            }
        } else if (playerPers.getRightState() == PlayerControllerPers.State.Grabbing || playerPers.getRightState() == PlayerControllerPers.State.DistanceGrabbing) {
            if (OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch).magnitude < minHitVelocity) {
                return;
            }
        } else {
            return;
        }

        // Set all the stones rigidbody to dynamic
        foreach (GameObject stone in stones)
        {
            // Make their parent as the parent of the wall
            stone.transform.parent = transform.parent;
            stone.GetComponent<Rigidbody>().isKinematic = false;
        }

        // And disable the collider
        GetComponent<Collider>().enabled = false;

        // Destroy the back collider
        Destroy(backCollider);

        // Destroy the wall
        destroyed = true;
    }

    public bool IsDestroyed()
    {
        return destroyed;
    }
}
