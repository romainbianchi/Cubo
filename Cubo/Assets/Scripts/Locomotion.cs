using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Locomotion : MonoBehaviour
{

	// Select the controller type
    public enum ControllerType {LeftController, RightController}
    public ControllerType controllerType = ControllerType.LeftController;


    [Header( "Maximum Distance" )]
	[Range( 2f, 30f )]
	// Store the maximum distance the player can teleport
	public float maximumTeleportationDistance = 15f;


    [Header( "Marker" )]
	// Store the refence to the marker prefab used to highlight the targeted point
	public GameObject markerPrefab;
	protected GameObject marker_prefab_instanciated;


    // Player
    [Header("Player")]
    public OVRPlayerController player;
    public PlayerControllerPers playerPers;
    private CharacterController character_controller;


    //Point de contact avec le sol
    public RaycastHit hit;

    //Raycast parameters
    private List<Vector3> positions;
    private LineRenderer lineRenderer;
    private Vector3 raycastDirection;
    private Vector3 raycastPosition;

   
    //Settings
    public int rayCastSpawnLimit = 200;
    public float rayCastLength = 0.1f;
    public float gravity = 7.5f;
    public float smooth = 0.01f;
    private int amountOfRaycastsSpawned;
    public float rayCastEnlargorFactor = 2.0f; // facteur pour regler les pb de collisions


    // Start is called before the first frame update
    void Start()
    {
         character_controller = player.GetComponent<CharacterController>();

        lineRenderer = GetComponent<LineRenderer>(); 
    }


    // Keep track of the teleportation state to prevent continuous teleportation
	protected bool teleportation_locked = false;

    // Keep track of the pointing state
	// protected bool left_pointing = false;
    // protected bool right_pointing = false;

    // Store Tp target point
    protected Vector3 target_point;


    // Update is called once per frame
    void Update()
    {
        // State check
        if (controllerType == ControllerType.LeftController && (playerPers.getLeftState() == PlayerControllerPers.State.Grabbing || playerPers.getLeftState() == PlayerControllerPers.State.DistanceGrabbing)) return;
        if (controllerType == ControllerType.RightController && (playerPers.getRightState() == PlayerControllerPers.State.Grabbing || playerPers.getRightState() == PlayerControllerPers.State.DistanceGrabbing)) return;
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
                    if ( marker_prefab_instanciated == null ) marker_prefab_instanciated = GameObject.Instantiate( markerPrefab, this.transform );
                    marker_prefab_instanciated.transform.position = target_point;

                    if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
                    {
                        // Tp the player
                        // character_controller.Move(target_point - this.transform.position);

                        // Or tp player using transform
                        player.transform.position = target_point;
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
                lineRenderer.startColor = Color.red;

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
                    if ( marker_prefab_instanciated == null ) marker_prefab_instanciated = GameObject.Instantiate( markerPrefab, this.transform );
                    marker_prefab_instanciated.transform.position = target_point;

                    if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                    {
                        // Tp the player
                        character_controller.Move(target_point - this.transform.position);
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
                lineRenderer.startColor = Color.red;

            }
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

        lineRenderer.startColor = Color.green;
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

            // Si le raycast touche un objet, on arrete le raycast
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

}