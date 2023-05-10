using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerGrabber : Grabber
{
    // Select the hand type to know wich button should be pressed
    public enum ControllerType {LeftController, RightController}
    public ControllerType controllerType = ControllerType.LeftController;

    // Check if the player is closing the hand
    public override bool HandClosing()
    {
        if (controllerType == ControllerType.LeftController) {
            return (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger) && OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) ||
                   (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) && OVRInput.Get(OVRInput.Button.PrimaryHandTrigger));
        }
        if (controllerType == ControllerType.RightController) {
            return (OVRInput.GetDown(OVRInput.Button.SecondaryHandTrigger) && OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)) ||
                   (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger));
        }
        return false;
    }

    // Check if the player is opening the hand
    public override bool HandOpening()
    {
        if (controllerType == ControllerType.LeftController) {
            return (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) && !OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger)) ||
                   (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.PrimaryHandTrigger));
        }
        if (controllerType == ControllerType.RightController){
            return (OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger) && !OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger)) ||
                   (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger) && !OVRInput.Get(OVRInput.Button.SecondaryHandTrigger));
        }
        return false;
    }
}
