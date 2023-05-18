using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCuboBehaviour : MonoBehaviour
{
    // Respawn position
    public GameObject respawnPosition;

    // Respawn distance
    public float respawnDistance = 2f;

    // Player controller
    public PlayerControllerPers playerController;

    // Box for cubo (to detect if the cubo is in the box or not with a collider)
    public GameObject box;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If the cubo is too far from the respawn position, respawn
        if (Vector3.Distance(transform.position, respawnPosition.transform.position) > respawnDistance)
        {
            // If the object is not grabbed by the player
            if (GetComponent<ObjectGrabbable>().IsAvailable()){
                Respawn();
            }
        }

        UpdateCuboState();
            
    }

    void Respawn()
    {
        // Respawn the cubo at the respawn position
        transform.position = respawnPosition.transform.position;

        // Reset the rotation (0,0,0)
        transform.rotation = Quaternion.identity;

        // Reset the velocity
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        
        // Reset the angular velocity
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    void UpdateCuboState()
    {
        // Available if not grabbed 
        bool available = GetComponent<ObjectGrabbable>().IsAvailable();

        // Immobile if velocity is null (or almost null)
        bool immobile = GetComponent<Rigidbody>().velocity.magnitude < 0.01f && GetComponent<Rigidbody>().angularVelocity.magnitude < 0.01f;

        // On place if is in the box
        bool inBox = box.GetComponent<BoxCollider>().bounds.Contains(transform.position);

        // If at least one face is down
        bool oneFaceDown = IsOneFaceDown();

        // Update the state of the cubo
        playerController.setCuboIsStable(available && immobile && inBox && oneFaceDown);
    }

    bool IsOneFaceDown(){

        // Get the rotation of the cubo
        Quaternion rotation = transform.rotation;

        // Get the rotation of the cubo in euler angles
        Vector3 eulerRotation = rotation.eulerAngles;

        // Get the rotation of the cubo in euler angles
        Vector3 eulerRotationAbs = new Vector3(Mathf.Abs(eulerRotation.x), Mathf.Abs(eulerRotation.y), Mathf.Abs(eulerRotation.z));

        // If at least two faces are close to 0° (modulo 90°) then the cubo is on one face )
        return eulerRotationAbs.x % 90 < 1f && eulerRotationAbs.y % 90 < 1f 
            || eulerRotationAbs.x % 90 < 1f && eulerRotationAbs.z % 90 < 1f 
            || eulerRotationAbs.y % 90 < 1f && eulerRotationAbs.z % 90 < 1f;
    }
}
