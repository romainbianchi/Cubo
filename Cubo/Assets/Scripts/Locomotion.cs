using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Locomotion : MonoBehaviour
{

	// Select the controller type
    public enum ControllerType {LeftController, RightController}
    public ControllerType controllerType = ControllerType.LeftController;


    [Header( "Materials")]
    public Material RedlineRendererMaterial;
    public Material GreenlineRendererMaterial;
    public Material GreenMarker;
    public Material RedMarker;
    public Material TransparentRed;
    public Material TransparentGreen;

    [Header( "Maximum Distance" )]
	[Range( 2f, 30f )]
	// Store the maximum distance the player can teleport
	public float maximumTeleportationDistance = 15f;


    [Header( "Teleportation" )]
	// Store the refence to the marker prefab used to highlight the targeted point
	public GameObject markerPrefab;
	protected GameObject marker_prefab_instanciated;
    private bool not_place_for_player = false;


    [Header("Player")]
    public OVRPlayerController player;
    public PlayerControllerPers playerPers;
    private CharacterController character_controller;


    [Header("Fade")]
    // To fade screen
    public GameObject centerEyeAnchor;


    //Point de contact avec le sol
    public RaycastHit hit;
    //Raycast parameters
    private List<Vector3> positions;
    private LineRenderer lineRenderer;
    private Vector3 raycastDirection;
    private Vector3 raycastPosition;
   
   [Header("Raycast parameters")]
    public int rayCastSpawnLimit = 200;
    public float rayCastLength = 0.1f;
    public float gravity = 7.5f;
    public float smooth = 0.01f;
    private int amountOfRaycastsSpawned;
    public float rayCastEnlargorFactor = 2.0f; // facteur pour regler les pb de collisions

    //Fade timer parameters
    protected float time = 0f;
    private float fade_time;


    // Start is called before the first frame update
    void Start()
    {
        character_controller = player.GetComponent<CharacterController>();
         
        fade_time = centerEyeAnchor.GetComponent<OVRScreenFade>().fadeTime;

        lineRenderer = GetComponent<LineRenderer>(); 

    }


    // Store the target point
    protected Vector3 target_point;


    // Update is called once per frame
    void Update()
    {

        // Check if the player is inside the cube
        if (!playerPers.isInCubo()) {
            if( marker_prefab_instanciated != null) Destroy(marker_prefab_instanciated);
            marker_prefab_instanciated = null;
            lineRenderer.enabled = false;
            return;
        }

        // Player is teleporting
        if (controllerType == ControllerType.LeftController && playerPers.getLeftState() == PlayerControllerPers.State.TpOnGoing) {
            handle_tp();
            return;
        }
        if (controllerType == ControllerType.RightController && playerPers.getRightState() == PlayerControllerPers.State.TpOnGoing){
            handle_tp();
            return;
        }
        
        // State check
        if (controllerType == ControllerType.LeftController && (playerPers.getLeftState() == PlayerControllerPers.State.Grabbing || playerPers.getLeftState() == PlayerControllerPers.State.DistanceGrabbing || playerPers.getRightState() == PlayerControllerPers.State.TpOnGoing)) return;
        if (controllerType == ControllerType.RightController && (playerPers.getRightState() == PlayerControllerPers.State.Grabbing || playerPers.getRightState() == PlayerControllerPers.State.DistanceGrabbing || playerPers.getLeftState() == PlayerControllerPers.State.TpOnGoing)) return;

        // Handle locomotion behavior
        handle_locomotion();
    }

    // Handle locomotion
    public void handle_locomotion()
    {

        // Left controller Tp
        if (controllerType == ControllerType.LeftController)
        {

            // If the player is pointing
            if (OVRInput.Get(OVRInput.Button.Three) && !playerPers.getRightTpPointing())
            {

                // Left controller occupied
                playerPers.setLeftState(PlayerControllerPers.State.Locomotion);

                // Left controller pointing state
                playerPers.setLeftTpPointing(true);

                // Vector3 valid target_point
                if (aim_with(out target_point))
                {

                    // Instantiate the marker prefab if it doesn't already exists and place it to the targeted position
                    if ( marker_prefab_instanciated == null ){ 
                        marker_prefab_instanciated = GameObject.Instantiate( markerPrefab, this.transform );
                        marker_prefab_instanciated.transform.GetChild(1).GetComponent<TpCollider>().setLocomotion(this);
                        
                    }
                    // Set target point
                    marker_prefab_instanciated.transform.position = target_point;
                    // Lock the rotation of the marker 
                    marker_prefab_instanciated.transform.rotation = Quaternion.identity;

                    // Set the material to red
                    lineRenderer.material = RedlineRendererMaterial;

                    // Set the marker prefab to red
                    marker_prefab_instanciated.GetComponent<Renderer>().material = RedMarker;
                    marker_prefab_instanciated.transform.GetChild(0).GetComponent<Renderer>().material = TransparentRed;

                    // Check if the player is too far from the target point
                    if (Vector3.Distance(player.transform.position, target_point) > maximumTeleportationDistance) return;

                    if (not_place_for_player) return;

                    // If the layer of the hit point is "sky", return
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Sky")) return;
                    
                    // Set the material to green
                    lineRenderer.material = GreenlineRendererMaterial;

                    // Set the marker prefab to green
                    marker_prefab_instanciated.transform.GetChild(0).GetComponent<Renderer>().material = TransparentGreen;
                    marker_prefab_instanciated.GetComponent<Renderer>().material = GreenMarker;

                    if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                    {
                        // Fade out
                        centerEyeAnchor.GetComponent<OVRScreenFade>().FadeOut();

                        // Update Tp state
                        playerPers.setLeftState(PlayerControllerPers.State.TpOnGoing);

                    }

                }

            } else {

                // Left controller available
                playerPers.setLeftState(PlayerControllerPers.State.Idle);

                // Left controller not pointing
                playerPers.setLeftTpPointing(false);

                // destroy the marker prefab if it exists
                if ( marker_prefab_instanciated != null ) Destroy( marker_prefab_instanciated);
                marker_prefab_instanciated = null;
                lineRenderer.enabled = false;

                // update the not_place_for_player state
                not_place_for_player = false;

            }

        }


        // Right controller Tp
        if (controllerType == ControllerType.RightController)
        {
            
            // If the player is pointing
            if (OVRInput.Get(OVRInput.Button.One) && !playerPers.getLeftTpPointing())
            {

                // Right controller occupied
                playerPers.setRightState(PlayerControllerPers.State.Locomotion);

                // Right controller pointing state
                playerPers.setRightTpPointing(true);

                // Vector3 valid target_point
                if (aim_with(out target_point))
                {

                    // Instantiate the marker prefab if it doesn't already exists and place it to the targeted position
                    if ( marker_prefab_instanciated == null ){ 
                        marker_prefab_instanciated = GameObject.Instantiate( markerPrefab, this.transform );
                        marker_prefab_instanciated.transform.GetChild(1).GetComponent<TpCollider>().setLocomotion(this);
                        
                    }
                    // Set target point
                    marker_prefab_instanciated.transform.position = target_point;
                    // Lock the rotation of the marker 
                    marker_prefab_instanciated.transform.rotation = Quaternion.identity;
                    
                    // Set the marker to red
                    lineRenderer.material = RedlineRendererMaterial;
                    marker_prefab_instanciated.GetComponent<Renderer>().material = RedMarker;
                    marker_prefab_instanciated.transform.GetChild(0).GetComponent<Renderer>().material = TransparentRed;

                    // Check if the player is too far from the target point
                    if (Vector3.Distance(player.transform.position, target_point) > maximumTeleportationDistance) return;

                    // check if there's enough space for the player
                    if (not_place_for_player) return;

                    // If the layer of the hit point is "sky", return
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Sky")) return;
                    
                    // Set the marker to green
                    lineRenderer.material = GreenlineRendererMaterial;
                    marker_prefab_instanciated.GetComponent<Renderer>().material = GreenMarker;
                    marker_prefab_instanciated.transform.GetChild(0).GetComponent<Renderer>().material = TransparentGreen;

                    if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                    {

                        // Fade out
                        centerEyeAnchor.GetComponent<OVRScreenFade>().FadeOut();

                        // Update tp state
                        playerPers.setRightState(PlayerControllerPers.State.TpOnGoing);
                  
                    }

                } 

            } else {

                // Right controller available
                playerPers.setRightState(PlayerControllerPers.State.Idle);

                // Right controller not pointing
                playerPers.setRightTpPointing(false);

                // destroy the marker prefab if it exists
                if ( marker_prefab_instanciated != null ) Destroy( marker_prefab_instanciated);
                marker_prefab_instanciated = null;
                lineRenderer.enabled = false;

                // update the not_place_for_player state
                not_place_for_player = false;

            }
        }

    }


    private void handle_tp(){

        // update time
        time += Time.deltaTime;

        // Lock the position of the marker
        marker_prefab_instanciated.transform.position = target_point;
        // Lock the rotation of the marker 
        marker_prefab_instanciated.transform.rotation = Quaternion.identity;        

        // if fade out is finished
        if (end_fade()){
            // update tp state
            if (controllerType == ControllerType.LeftController) playerPers.setLeftState(PlayerControllerPers.State.Idle);
            if (controllerType == ControllerType.RightController) playerPers.setRightState(PlayerControllerPers.State.Idle);
            // reset time
            time = 0f;
            // tp player
            character_controller.enabled = false;
            character_controller.transform.position = target_point + new Vector3(0, character_controller.height/2, 0);
            character_controller.enabled = true;
            //Play step sound
            playerPers.playStepSound();
            // Fade in
            centerEyeAnchor.GetComponent<OVRScreenFade>().FadeIn();
        }
    }


	protected bool aim_with (out Vector3 target_point) {

		// Default the "output" target point to the null vector
		target_point = new Vector3();

		// Launch the ray cast and leave if it doesn't hit anything
		RaycastHit hit;
		hit = getHit();

		// If the aimed point is out of range (i.e. the raycast distance is above the maximum distance) then prevent the teleportation
		if ( hit.distance > maximumTeleportationDistance ) return false;

		// "Output" the target point
		target_point = hit.point;
		return true;
	}


    private RaycastHit getHit()
    {
        // Ici on obtient la liste de points de la parabole (et on lance le raycast a l'intérieur de la fonction)
        Vector3[] curvePoints = curvedRaycast();

        // Ici on update les parametre du line renderer en fonction de la liste de points
        int numPoints = curvePoints.Length;
        lineRenderer.positionCount = numPoints;

        // Ici on update les positions du line renderer en fonction de la liste de points
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 point = curvePoints[i];
            lineRenderer.SetPosition(i, point);
        }

        lineRenderer.enabled = true;

        return hit;

    }


    private Vector3[] curvedRaycast()
    {
        amountOfRaycastsSpawned = 0;
        positions = new List<Vector3>();
        raycastPosition = transform.position;
        raycastDirection = transform.forward;
        
        // Tant que le nombre de raycast est inferieur au nombre de raycast max on continue a creer la parabole
        while (amountOfRaycastsSpawned < rayCastSpawnLimit)
        {
            positions.Add(raycastPosition);

            // Si le raycast touche un objet, on arrete le raycast (sauf si c'est un objet ignore raycast)
            if (Physics.Raycast(raycastPosition, raycastDirection, out hit, rayCastEnlargorFactor*rayCastLength))
            {
                positions.Add(hit.point);
                break;
            }
            
            // Sinon on append le raycast
            raycastPosition += raycastDirection * rayCastLength;
            raycastDirection += new Vector3(0.0f, -gravity * smooth, 0.0f);
            
            amountOfRaycastsSpawned++;
        }
        // On retourne la liste de points pour afficher le line renderer apres l'appel de la fonction
        return positions.ToArray();
    }

    private bool end_fade()
    {
        if (time > fade_time) return true;

        return false;
    }

    public void set_not_place_for_player(bool state){

        not_place_for_player = state;

    }

}
