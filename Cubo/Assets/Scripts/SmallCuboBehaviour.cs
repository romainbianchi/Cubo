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

    // IceBlock
    public GameObject iceBlock;
    public GameObject meltedIceBlock;
    public Collider houseCollider;
    private bool iceMelted = false;
    

    // Cubo state
    private bool stable = false;

    // Store the 6 faces of the cubo
    private GameObject[] faces = new GameObject[6];
    private GameObject currentFace = null;

    // Bool to block sound 
    private bool soundPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Store the faces
        for (int i = 0; i < 6; i++)
        {
            faces[i] = transform.GetChild(i).gameObject;
        } 

        iceBlock.SetActive(true);
        meltedIceBlock.SetActive(false);
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

        UpdateCurrentFace();
        UpdateCuboState();
        
        // Debug print the current face
        // Debug.Log(FaceDown());
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
        bool oneFaceDown = currentFace != null;

        stable = available && immobile && inBox && oneFaceDown;

        //Play sound once
        if (stable) {
            if (!soundPlayed) {
                GetComponent<AudioSource>().Play();
                soundPlayed = true;
            }
        } else {
            soundPlayed = false;
        }

        // Update the state of the cubo
        playerController.setCuboIsStable(stable);
    }

    void UpdateCurrentFace(){

        // Which face has is up transform y <= -0.99f (upside down)
        foreach (GameObject face in faces)
        {
            if (face.transform.up.y <= -0.99f)
            {
                currentFace = face;
                return;
            }
        }

        // If no face is down, set currentFace to null
        currentFace = null;
    }

    public string FaceDown(){
        if(currentFace == null) return null;

        return currentFace.name;
    }

    public void meltIceBlock(){
        iceBlock.SetActive(false);
        meltedIceBlock.SetActive(true);
        houseCollider.enabled = false;

        iceMelted = true;
    }

    public bool getIceIsMelted(){
        return iceMelted;
    }
}
