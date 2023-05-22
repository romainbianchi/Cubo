using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceGrabbable : MonoBehaviour
{
    // List of imortant objects
    public enum ImportantObject { None, Stick, Torch, Cup, Key, CuboToBeContinued };
    public ImportantObject importantObject = ImportantObject.None;

    // If important object is not none, we have to specify the respawn position
    public Transform respawnPosition;

    // Material when the object is pointed
    public Material pointedMaterial;
    private Material originMaterial;
    private bool pointed = false;
    

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

        // Set the material as the saved material
        originMaterial = GetComponent<Renderer>().material;
    }

    // When the object is grasped: set the anchor as the child of the hand
    public void AttachTo(DistanceGrabber grabbedBy, Vector3 hitPoint)
    {
        // Set the availability to false
        available = false;
        if(GetComponent<ObjectGrabbable>() != null) GetComponent<ObjectGrabbable>().SetAvailable(false);
        

        // Save the hand to which the anchor is attached
        attachedGrabber = grabbedBy;

        // Set the parent of the anchor to the hand
        transform.SetParent(grabbedBy.transform);

        // Offset the position of the anchor to the hit point
        Vector3 offset = transform.position - hitPoint;

        // Set position to the hand + forward offset
        transform.position = grabbedBy.transform.position + offset;

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
        if(GetComponent<ObjectGrabbable>() != null) GetComponent<ObjectGrabbable>().SetAvailable(true);

        // Set the attached grabber to null
        attachedGrabber = null;

        // If dropped in desk: set parent to null
        if (transform.position.x < 10f) {
            transform.SetParent(null);
        } else {
            transform.SetParent(initialParent);
        }
        
        // If not important object, then drop it normally
        if (importantObject == ImportantObject.None || transform.position.x > 10f) {
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
        } else {
                transform.position = respawnPosition.position;
                transform.rotation = respawnPosition.rotation;
        }
    }

    public void IsPointed(bool state){
        pointed = state;

        if (pointed)
        {
            // Paint only if the object is available
            if (!available) return;            

            // Set the material as the pointed material
            GetComponent<Renderer>().material = pointedMaterial;
        }
        else
        {
            // Set the material as the saved material
            GetComponent<Renderer>().material = originMaterial;
        }
    }
    
    public void SetAvailable(bool state){
        available = state;
    }

}
    
