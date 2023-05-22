using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceGrabber : MonoBehaviour
{

    // Select the hand type to know wich button should be pressed
    public enum ControllerType {LeftController, RightController}
    public ControllerType controllerType = ControllerType.LeftController;

    // Set the maximum distance to grab an object
    public float maximumGrabDistance = 5.0f;

    // Store all gameobjects containing an Anchor
    // N.B. As the list of Anchors is the same between all Hand Controllers this field can
    // be made static to prevent having the same list stored in each Hand Controller instance.
    static protected DistanceGrabbable[] objectsGrabbable = null;

    // Store the object pointed by the player
    protected GameObject pointedObject;
    protected GameObject previousPointedObject;

    // Line renderer parameters
    private LineRenderer lineRenderer;
    public Material redMaterial;
    public Material greenMaterial;

    // Player
    [Header("Player")]
    public PlayerControllerPers playerPers;

    // Start is called before the first frame update
    void Start()
    {
        // Find all gameobjects containing an Anchor if the list is empty
        if (objectsGrabbable == null) objectsGrabbable = GameObject.FindObjectsOfType<DistanceGrabbable>();

        lineRenderer = GetComponent<LineRenderer>();

        pointedObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is not in cubo, return
        if (playerPers.developerMode == false){
            if (!playerPers.isInCubo() 
                && !((controllerType == ControllerType.LeftController && playerPers.getLeftState() == PlayerControllerPers.State.DistanceGrabbing)
                || (controllerType == ControllerType.RightController && playerPers.getRightState() == PlayerControllerPers.State.DistanceGrabbing))) {
                lineRenderer.enabled = false;
                return;
            }
        }

        // State check
        if (controllerType == ControllerType.LeftController && (playerPers.getLeftState() == PlayerControllerPers.State.Grabbing || playerPers.getLeftState() == PlayerControllerPers.State.Locomotion || playerPers.getLeftState() == PlayerControllerPers.State.TpOnGoing)) return;
        if (controllerType == ControllerType.RightController && (playerPers.getRightState() == PlayerControllerPers.State.Grabbing || playerPers.getRightState() == PlayerControllerPers.State.Locomotion || playerPers.getRightState() == PlayerControllerPers.State.TpOnGoing)) return;
        
        // At each frame, handle the controller grasp behaviour
        HandleGrabBehaviour();
    }

    bool Aim()
    {
        if (grabbedObject != null) return false;

        // Check if the player is pressing button 2 or 4
        if (controllerType == ControllerType.LeftController && !OVRInput.Get(OVRInput.Button.Four)) return false;
        if (controllerType == ControllerType.RightController && !OVRInput.Get(OVRInput.Button.Two)) return false;

        return true;
    }

    // Check if the player is aiming at an object
    bool RayHit(out Vector3 target)
    {
        target = Vector3.zero;

        // Launch the ray cast and leave if it doesn't hit anything
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.forward, out hit, maximumGrabDistance)) return false;

        // Save the hit point
        target = hit.point;
        
        // Check if the object is too far
        if (hit.distance > maximumGrabDistance) return false;

        // Check if an grabbable object is hit
        if (hit.collider.gameObject.GetComponent<DistanceGrabbable>() == null) return false;

        // Save the object hit
        pointedObject = hit.collider.gameObject;

        // Check if the object is available
        if (!pointedObject.GetComponent<DistanceGrabbable>().IsAvailable()) return false;

        return true;
    }

    // Check if the player is closing the hand
    bool HandClosing()
    {
        if (controllerType == ControllerType.LeftController) {
            return (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger));
        }
        if (controllerType == ControllerType.RightController) {
            return (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger));
        }
        return false;
    }

    // Check if the player is opening the hand
    // Check if the player is opening the hand
    bool HandOpening()
    {
        if (controllerType == ControllerType.LeftController) {
            return (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger));
        }
        if (controllerType == ControllerType.RightController){
            return (OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger));
        }
        return false;
    }

    // Display a ray out from the controller forward
    void DisplayRay(Vector3 hitPoint)
    {
        // Enable the line renderer
        lineRenderer.enabled = true;

        // lineRenderer length = 2
        lineRenderer.positionCount = 2;

        // Set the starting and ending points of the line
        lineRenderer.SetPosition(0, transform.position);

        // If the ray hit something, then set the end of the line to the hit point
        if (hitPoint != Vector3.zero) lineRenderer.SetPosition(1, hitPoint);

        // Else set the end of the line to the maximum distance
        else lineRenderer.SetPosition(1, transform.position + transform.forward * maximumGrabDistance);
	}

    // We want to save the anchor that is being grasped
    protected DistanceGrabbable grabbedObject = null;

    void HandleGrabBehaviour()
    {
        if (Aim()){
           
            // Update state
            if (controllerType == ControllerType.LeftController) playerPers.setLeftState(PlayerControllerPers.State.DistanceGrabbing);
            if (controllerType == ControllerType.RightController) playerPers.setRightState(PlayerControllerPers.State.DistanceGrabbing);

            // Set the line renderer color to red
            lineRenderer.material = redMaterial;

            Vector3 hitPoint = Vector3.zero;
            pointedObject = null;

            if (RayHit(out hitPoint)) // This function updates the hitpoint and pointedObject
            {
                // Set the line renderer color to green
                lineRenderer.material = greenMaterial;

                // Grab the object if the hand is closing
                if (HandClosing()) GrabObject(hitPoint);
            }

            // If the object pointed has changed, then update the previous object
            if (pointedObject != previousPointedObject && previousPointedObject != null) previousPointedObject.GetComponent<DistanceGrabbable>().IsPointed(false);
            
            // If the object pointed is not null, then update the object
            if (pointedObject != null) pointedObject.GetComponent<DistanceGrabbable>().IsPointed(true);

            // Update the previous object
            previousPointedObject = pointedObject;

            DisplayRay(hitPoint);

        } else {
            lineRenderer.enabled = false;
            
            if (pointedObject != null) pointedObject.GetComponent<DistanceGrabbable>().IsPointed(false);
            pointedObject = null;
            
            // Release the object if the hand is opening
            if (HandOpening()) ReleaseObject();

            // Update state
            if (grabbedObject == null){
                if (controllerType == ControllerType.LeftController) playerPers.setLeftState(PlayerControllerPers.State.Idle);
                if (controllerType == ControllerType.RightController) playerPers.setRightState(PlayerControllerPers.State.Idle);
            }
        }
    }

    void GrabObject(Vector3 hitPoint){
        // If there is a closest object
        if (pointedObject != null) {
            // Then save it as the grasped anchor
            grabbedObject = pointedObject.GetComponent<DistanceGrabbable>();
            // And attach the anchor to the hand
            grabbedObject.AttachTo(this, hitPoint);
        }
        
    }

    public void ReleaseObject(){        
        if (grabbedObject != null){
            grabbedObject.DetachFrom(this);
            grabbedObject = null;
        }
    }
}
