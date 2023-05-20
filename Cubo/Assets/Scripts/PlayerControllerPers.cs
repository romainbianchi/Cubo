using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerPers : MonoBehaviour
{
    // Refer to a position of in_cubo
    public GameObject inCuboPos;
    public GameObject inDeskPos;

    // States
    public enum State {Locomotion, TpOnGoing, DistanceGrabbing, Grabbing, Idle}
    private State right_state = State.Idle;
    private State left_state = State.Idle;

    // Controllers pointing states
    private bool left_tp_pointing = false;
    private bool right_tp_pointing = false;
    private bool cuboIsStable = false;

    // Check if the player is in cubo
    private bool inCubo = false;
    
    public void TpPlayerInCubo()
    {
        // TP the player in cubo
        if (!cuboIsStable) return;
        this.GetComponent<CharacterController>().enabled = false;
        transform.position = inCuboPos.transform.position;
        this.GetComponent<CharacterController>().enabled = true;

        // Set all of the object of type Grabbable and distance grabbable to dynamic (BUG A REVOIR: LUDO)
        // SetCuboObjectsKinematic(false);          
    }

    public void TpPlayerInDesk()
    {
        // TP the player in desk
        this.GetComponent<CharacterController>().enabled = false;
        transform.position = inDeskPos.transform.position;
        this.GetComponent<CharacterController>().enabled = true;

        // Set all of the object of type Grabbable and distance grabbable to kinematic (BUG A REVOIR: LUDO)
        // SetCuboObjectsKinematic(true);
    }

    // Set and get Tp pointing states
    public void setLeftTpPointing(bool state)
    {
        left_tp_pointing = state;
    }

    public void setRightTpPointing(bool state)
    {
        right_tp_pointing = state;
    }

    public bool getLeftTpPointing()
    {
        return left_tp_pointing;
    }

    public bool getRightTpPointing()
    {
        return right_tp_pointing;
    }

    // Set and get states
    public void setRightState(State state)
    {
        right_state = state;
    }

    public State getRightState()
    {
        return right_state;
    }

    public void setLeftState(State state)
    {
        left_state = state;
    }

    public State getLeftState()
    {
        return left_state;
    }

    public void setCuboIsStable(bool state)
    {
        cuboIsStable = state;
    }

    public bool getCuboIsStable()
    {
        return cuboIsStable;
    }

    public bool isInCubo()
    {
        return inCubo;
    }

	void Start () 
    { 
        
        // Find all gameobjects with script ObjectGrabbable (BUG A REVOIR: LUDO)
        // objectsGrabbable = GameObject.FindGameObjectsWithTag("Grabbable");

        // Set all of the object of type Grabbable and distance grabbable to kinematic
        // SetCuboObjectsKinematic(true);
    }

    void Update()
    {
        inCubo = (transform.position.x > 3.5f);        
    } 



    // BUG A REVOIR: LUDO
    // EN GROS JE VOULAIS SET STATIC LES OBJETS DANS CUBO QUAND ON TP DANS LE DESK,
    // ET LES REMETTRE EN DYNAMIC QUAND ON TP DANS CUBO MAIS CA MARCHE PAS TROP TROP,
    // CA FAIT BUGGER LE BOUTON POUR AUCUNE RAISON, JARRIVE PAS A VOIR D'OU CA VIENT


    // GameObject[] objectsGrabbable;
    // void SetCuboObjectsKinematic(bool state)
    // {
    //     // Set all of the object of layer Grabbable to kinematic
    //     foreach (GameObject obj in objectsGrabbable)
    //     {
    //         // If the obejct is not available, skip it
    //         if (obj.GetComponent<ObjectGrabbable>().IsAvailable() == false) continue;
            
    //         if (obj.transform.position.x > 3.5f) obj.GetComponent<Rigidbody>().isKinematic = state;
    //     }
    // }
}
