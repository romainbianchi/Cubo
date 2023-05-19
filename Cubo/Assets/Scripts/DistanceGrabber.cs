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

    private LineRenderer lineRenderer;

    // Player
    [Header("Player")]
    public PlayerControllerPers playerPers;

    // Start is called before the first frame update
    void Start()
    {
        // Find all gameobjects containing an Anchor if the list is empty
        if (objectsGrabbable == null) objectsGrabbable = GameObject.FindObjectsOfType<DistanceGrabbable>();

        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // State check
        if (controllerType == ControllerType.LeftController && (playerPers.getLeftState() == PlayerControllerPers.State.Grabbing || playerPers.getLeftState() == PlayerControllerPers.State.Locomotion || playerPers.getLeftState() == PlayerControllerPers.State.TpOnGoing)) return;
        if (controllerType == ControllerType.RightController && (playerPers.getRightState() == PlayerControllerPers.State.Grabbing || playerPers.getRightState() == PlayerControllerPers.State.Locomotion || playerPers.getRightState() == PlayerControllerPers.State.TpOnGoing)) return;
        // At each frame, handle the controller grasp behaviour
        HandleGrabBehaviour();
    }

    // Check if the player is aiming at an object
    bool Aim(out Vector3 target)
    {
        
        target = new Vector3();

        if (grabbedObject != null) return false;

        if (controllerType == ControllerType.LeftController && !OVRInput.Get(OVRInput.Button.Four)) return false;
        if (controllerType == ControllerType.RightController && !OVRInput.Get(OVRInput.Button.Two)) return false;

        // Launch the ray cast and leave if it doesn't hit anything
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.forward, out hit, maximumGrabDistance)) return false;

        // "Output" the target point
        target = hit.point;

        if (hit.distance > maximumGrabDistance) return false;

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

    // Display a ray out from the controller
    void DisplayRay(Vector3 target)
    {
        // Enable the line renderer
        lineRenderer.enabled = true;

        // lineRenderer length = 2
        lineRenderer.positionCount = 2;

        // Set the starting and ending points of the line
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, target);
	}

    void SpawnSphere()
    {
        // Spawn a sphere 1m in front of the controller
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = transform.position + transform.forward/5;

        // Scale the sphere by 10%
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
         
        // Add a rigidbody to the sphere
        Rigidbody rb = sphere.AddComponent<Rigidbody>();
        rb.mass = 0.1f;
        rb.useGravity = true;

        // Throw the sphere forward
        rb.AddForce(transform.forward * 30);

        // Add a collider to the sphere
        SphereCollider sc = sphere.AddComponent<SphereCollider>();
        sc.radius = 0.01f;
    }

    // We want to save the anchor that is being grasped
    protected DistanceGrabbable grabbedObject = null;

    void HandleGrabBehaviour()
    {
        if (Aim(out Vector3 target)){
            DisplayRay(target);
            GrabObject(target);
            // Update state
            if (controllerType == ControllerType.LeftController) playerPers.setLeftState(PlayerControllerPers.State.DistanceGrabbing);
            if (controllerType == ControllerType.RightController) playerPers.setRightState(PlayerControllerPers.State.DistanceGrabbing);
        } else {
            lineRenderer.enabled = false;
            ReleaseObject();
            if (grabbedObject == null){
                // Update state
                if (controllerType == ControllerType.LeftController) playerPers.setLeftState(PlayerControllerPers.State.Idle);
                if (controllerType == ControllerType.RightController) playerPers.setRightState(PlayerControllerPers.State.Idle);
            }
        }
    }

    void GrabObject(Vector3 target){
        if (!HandClosing()) return;

        // Determine which object available is the closest from the target
        float minDistance = float.MaxValue;
        DistanceGrabbable closestObject = null;
        foreach (DistanceGrabbable objectGrabbable in objectsGrabbable)
        {
            // Skip if the object is not available
            if (!objectGrabbable.IsAvailable()) continue;

            // Compute the distance between the object and the hand
            float distance = Vector3.Distance(objectGrabbable.transform.position, target);

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

    void ReleaseObject(){
        if (!HandOpening()) return;
        
        if (grabbedObject != null){
            grabbedObject.DetachFrom(this);
            grabbedObject = null;
        }
    }
}
