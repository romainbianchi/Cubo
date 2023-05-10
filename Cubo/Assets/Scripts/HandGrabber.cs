using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRHand;

public class HandGrabber : Grabber
{
    // Store the hand to track
    public OVRHand hand;
    
    // Check that the hand was not already pinching or opened
    protected bool wasPinching = false;

    // Check if the player is closing the hand
    public override bool HandClosing()
    {
        if (wasPinching) return false;
        wasPinching = IsPinching();
        return IsPinching();
    }

    // Check if the player is opening the hand
    public override bool HandOpening()
    {
        if (!wasPinching) return false;
        wasPinching = IsPinching();
        return !IsPinching();
    }

    bool IsPinching()
    {
        return hand.GetFingerIsPinching(HandFinger.Index);
    }
}
