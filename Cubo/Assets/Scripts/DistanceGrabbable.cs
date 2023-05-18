using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceGrabbable : MonoBehaviour
{
    // First set the grabbing radius of the anchor
    public float grabbingRadius = 0.1f;

    // Then set the availability of the anchor
    private bool available;
    public bool IsAvailable()
    {
        return available;
    }
    
    // Save the grabber to which the object is attached
    private DistanceGrabber attachedGrabber;

    // Save the initial parent when the anchor is created
    private Transform initialParent;
    void Start()
    {
        initialParent = transform.parent;
        available = true;
        attachedGrabber = null;

        // Set the layer as "Grabbable"
        gameObject.layer = LayerMask.NameToLayer("Grabbable");
    }

    // When the object is grasped: set the anchor as the child of the hand
    public void AttachTo(DistanceGrabber grabbedBy)
    {
        // Set the availability to false
        available = false;

        // Save the hand to which the anchor is attached
        attachedGrabber = grabbedBy;

        // Set the parent of the anchor to the hand
        transform.SetParent(grabbedBy.transform);

        // Set position to the hand + forward offset
        transform.position = grabbedBy.transform.position + grabbedBy.transform.forward * grabbingRadius;

        // Set the object as kinematic
        GetComponent<Rigidbody>().isKinematic = true;
    }

    // When the object is released: set the anchor as the child of the initial parent
    public void DetachFrom(DistanceGrabber grabbedBy)
    {
        // If the hand is not the one that attached the anchor, then do nothing
        if (grabbedBy != attachedGrabber) return;

        // Set the object as not kinematic
        GetComponent<Rigidbody>().isKinematic = false;

        // Set the availability to true
        available = true;

        // Set the attached grabber to null
        attachedGrabber = null;

        // Set the parent of the anchor to the initial parent
        transform.SetParent(initialParent);

        
        if (grabbedBy.controllerType == DistanceGrabber.ControllerType.LeftController)
        {
            GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
            GetComponent<Rigidbody>().angularVelocity = -OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);
        }
        else if (grabbedBy.controllerType == DistanceGrabber.ControllerType.RightController)
        {
            GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            GetComponent<Rigidbody>().angularVelocity = -OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
        }
    }
}
    
