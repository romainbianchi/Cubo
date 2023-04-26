using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRHand;

public class ObjectAnchor : MonoBehaviour
{
    // First set the grasping radius of the anchor
    public float grasping_radius = 0.1f;

    // Then set the availability of the anchor
    private bool available = true;
    public bool is_available()
    {
        return available;
    }

    // Save the initial parent when the anchor is created
    private Transform initial_parent;
    void Start()
    {
        initial_parent = transform.parent;
    }
    

    // Save the hand to which the anchor is attached
    private HandController attached_hand;

    // When the object is grasped: set the anchor as the child of the hand
    public void attach_to(HandController hand)
    {
        // Set the availability to false
        available = false;

        // Save the hand to which the anchor is attached
        attached_hand = hand;

        // Set the parent of the anchor to the hand
        transform.SetParent(hand.transform);

        // Set the object as kinematic
        GetComponent<Rigidbody>().isKinematic = true;


    }

    // When the object is released: set the anchor as the child of the initial parent
    public void detach(HandController hand)
    {
        // If the hand is not the one that attached the anchor, then do nothing
        if (hand != attached_hand) return;

        // Set the object as not kinematic
        GetComponent<Rigidbody>().isKinematic = false;

        // Set the availability to true
        available = true;

        // Set the attached hand to null
        attached_hand = null;

        // Set the parent of the anchor to the initial parent
        transform.SetParent(initial_parent);

        // Make the object follow the trajectory of the correct hand type
        if(hand.handType == HandController.HandType.LeftController) {
            GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.LTouch);
            GetComponent<Rigidbody>().angularVelocity = -OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.LTouch);
        } else if(hand.handType == HandController.HandType.RightController) {
            GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(OVRInput.Controller.RTouch);
            GetComponent<Rigidbody>().angularVelocity = -OVRInput.GetLocalControllerAngularVelocity(OVRInput.Controller.RTouch);
        }
    }
}