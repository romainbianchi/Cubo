using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRHand;
using static OVRSkeleton;

public class ObjectGrabbable : MonoBehaviour
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
    private Grabber attachedGrabber;

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
    public void AttachTo(Grabber grabbedBy)
    {
        // Set the availability to false
        available = false;

        // Save the hand to which the anchor is attached
        attachedGrabber = grabbedBy;

        // Lift the object a little bit
        transform.position += Vector3.up * 0.03f;

        // Set the parent of the anchor to the hand
        transform.SetParent(grabbedBy.transform);

        // Set the object as kinematic
        GetComponent<Rigidbody>().isKinematic = true;

    }

    // When the object is released: set the anchor as the child of the initial parent
    public void DetachFrom(Grabber grabbedBy)
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
        
        if (grabbedBy.controllerType == Grabber.ControllerType.LeftController)
        {
            GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
            GetComponent<Rigidbody>().angularVelocity = -OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);
        }
        else if (grabbedBy.controllerType == Grabber.ControllerType.RightController)
        {
            GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            GetComponent<Rigidbody>().angularVelocity = -OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
        }
        // Because thee Oculus plugin does not provide informations about
        // the speed of the hands when they are not controllers, we cannot 
        // make the object follow the trajectory if released by a hand.
        // However, we could get the position of the idextip, at two different 
        // frames, and compute the speed of the hand by deviding the distance
        // between the two positions by the time elapsed between the two frames 
        // (= compputig the speed ourselves).
    }
}