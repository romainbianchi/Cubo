using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Grabber : MonoBehaviour
{
    // Store all gameobjects containing an Anchor
    // N.B. As the list of Anchors is the same between all Hand Controllers this field can
    // be made static to prevent having the same list stored in each Hand Controller instance.
    static protected ObjectGrabbable[] objectsGrabbable = null;

    // Start is called before the first frame update
    void Start()
    {
        // Find all gameobjects containing an Anchor if the list is empty
        if (objectsGrabbable == null) objectsGrabbable = GameObject.FindObjectsOfType<ObjectGrabbable>();
    }

    // Update is called once per frame
    void Update()
    {
        // At each frame, handle the controller grasp behaviour
        HandleGrabBehaviour();
    }

    // Check if the player is closing the hand
    abstract public bool HandClosing();
    
    // Check if the player is opening the hand
    abstract public bool HandOpening();

    // We want to save the anchor that is being grasped
    protected ObjectGrabbable grabbedObject = null;

    public void HandleGrabBehaviour()
    {
        // If the hand is not closing, and not opening, then skip
        if (!HandClosing() && !HandOpening()) return;

        // If the hand closes
        if (HandClosing())
        {
            // Determine which object available is the closest from the hand
            float minDistance = float.MaxValue;
            ObjectGrabbable closestObject = null;
            foreach (ObjectGrabbable objectGrabbable in objectsGrabbable)
            {
                // Skip if the object is not available
                if (!objectGrabbable.IsAvailable()) continue;

                // Compute the distance between the object and the hand
                float distance = Vector3.Distance(objectGrabbable.transform.position, transform.position);

                // If the hand is in the grasping radius of the object, and the distance is smaller than the previous one
                if (distance < objectGrabbable.grabbingRadius && distance < minDistance)
                {
                    // Then save the object as the closest one
                    closestObject = objectGrabbable;
                    // And save the distance as the new minimum distance
                    minDistance = distance;
                }
            }
            // If there is a closest object
            if (closestObject != null) {
                // Then save it as the grasped anchor
                grabbedObject = closestObject;
                // And attach the anchor to the hand
                grabbedObject.AttachTo(this);
            }
        }

        // If the hand opens
        else if (HandOpening())
        {
            // If there is a grasped anchor
            if (grabbedObject != null)
            {
                // Then detach the anchor from the hand
                grabbedObject.DetachFrom(this);
                // And set the grasped anchor to null
                grabbedObject = null;
            }
        }
    }
}
