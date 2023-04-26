using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRHand;

public class HandController : MonoBehaviour
{

    // Select the hand type to know wich button should be pressed, or wich fingers should be pinched
    public enum HandType {LeftController, RightController, LeftHand, RightHand}
    public HandType handType;

    // The hand
    public OVRHand hand;

    // Store all gameobjects containing an Anchor
    // N.B. As the list of Anchors is the same between all Hand Controllers this field can
    // be made static to prevent having the same list stored in each Hand Controller instance.
    static protected ObjectAnchor[] anchors;

    // Start is called before the first frame update
    void Start()
    {
        // Find all gameobjects containing an Anchor if the list is empty
        if (anchors == null || anchors.Length == 0) anchors = FindObjectsOfType<ObjectAnchor>();
    }

    // Update is called once per frame
    void Update()
    {
        // At each frame, handle the controller grasp behaviour
        handle_grasp_behaviour();
    }

    // Check if the player is closing the hand
    public bool is_hand_closed()
    {
        // If the hand type is left controller
        if (handType == HandType.LeftController)
        {
            // Return true if the left hand trigger is pressed
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.5f) return true; 
        }
        // If the hand type is right controller or right hand
        else if (handType == HandType.RightController)
        {
            // Return true if the right hand trigger is pressed
            if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.5f) return true;
        }
        // If the hand type is left hand or right hand
        else if (handType == HandType.LeftHand || handType == HandType.RightHand)
        {
            // Return true if the hand is pinching the index finger or middle finger or any other finger
            if (hand.GetFingerIsPinching(OVRHand.HandFinger.Index)
                || hand.GetFingerIsPinching(OVRHand.HandFinger.Middle)
                || hand.GetFingerIsPinching(OVRHand.HandFinger.Ring)
                || hand.GetFingerIsPinching(OVRHand.HandFinger.Pinky)
                || hand.GetFingerIsPinching(OVRHand.HandFinger.Thumb)) return true;

        }

        // Return false
        return false;
    }

    // ########################## //
    // Handle the grasp behaviour //
    // ########################## //

    // We want to detect the edge of the grasp, so we need to know the previous state of the grasp
    protected bool previous_hand_closed = false;

    // We want to save the anchor that is being grasped
    protected ObjectAnchor grasped_anchor = null;

    void handle_grasp_behaviour()
    {
        // First, check if there is a grasp
        bool hand_closed = is_hand_closed();

        // If the hand was already closed then do nothing
        if (hand_closed == previous_hand_closed) return;

        // Then set the previous hand closed state to the current one
        previous_hand_closed = hand_closed;

        // If the hand closes
        if (hand_closed)
        {
            // Determine which object available is the closest from the hand
            float min_distance = float.MaxValue;
            ObjectAnchor closest_anchor = null;
            foreach (ObjectAnchor anchor in anchors)
            {
                // Skip if the anchor is not available
                if (!anchor.is_available()) continue;

                // Compute the distance between the anchor and the hand
                float distance = Vector3.Distance(anchor.transform.position, transform.position);

                // If the hand is in the graspinf radius of the anchor, and the distance is smaller than the previous one
                if (distance < anchor.grasping_radius && distance < min_distance)
                {
                    // Then save the anchor as the closest one
                    closest_anchor = anchor;
                    // And save the distance as the new minimum distance
                    min_distance = distance;
                }
            }

            // If there is a closest anchor
            if (closest_anchor != null) {
                // Then save it as the grasped anchor
                grasped_anchor = closest_anchor;
                // And attach the anchor to the hand
                grasped_anchor.attach_to(this);
            }
        }

        // If the hand opens
        else
        {
            // If there is a grasped anchor
            if (grasped_anchor != null)
            {
                // Then detach the anchor from the hand
                grasped_anchor.detach(this);
                // And set the grasped anchor to null
                grasped_anchor = null;
            }
        }
    }

    
}
