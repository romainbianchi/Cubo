using UnityEngine;

public class ObjectAnchorHO3 : MonoBehaviour {

	[Header( "Grasping Properties" )]
	public float graspingRadius = 0.1f;

	// Store initial transform parent
	protected Transform initial_transform_parent;
	void Start () {
		initial_transform_parent = transform.parent;
	}


	// Store the hand controller this object will be attached to
	protected HandController hand_controller = null;

	public void attach_to ( HandController hand_controller ) {
		// Store the hand controller in memory
		this.hand_controller = hand_controller;

		// Set the object to be placed in the hand controller referential
		transform.SetParent( hand_controller.transform );

		// Force the object to be kinematic
		GetComponent<Rigidbody>().isKinematic = true;

	}

	public void detach_from ( HandController hand_controller ) {
		// Make sure that the right hand controller ask for the release
		if ( this.hand_controller != hand_controller ) return;

		// Detach the hand controller
		this.hand_controller = null;

		// Set the object to be placed in the original transform parent
		transform.SetParent( initial_transform_parent );

		// Force the object to be non kinematic
		GetComponent<Rigidbody>().isKinematic = false;

		// Make the object follow his trajectory even when released
		GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity( hand_controller.handType == HandController.HandType.LeftHand ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch );
		GetComponent<Rigidbody>().angularVelocity = -OVRInput.GetLocalControllerAngularVelocity( hand_controller.handType == HandController.HandType.LeftHand ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch );
	}

	public bool is_available () { return hand_controller == null; }

	public float get_grasping_radius () { return graspingRadius; }
}