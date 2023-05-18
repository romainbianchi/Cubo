using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerPers : MonoBehaviour
{
    // Refer to a position of in_cubo
    public GameObject inCuboPos;
    public GameObject inDeskPos;

    // States
    public enum State {Locomotion, DistanceGrabbing, Grabbing, Idle}
    private State right_state = State.Idle;
    private State left_state = State.Idle;

    // Controllers pointing states
    private bool left_tp_pointing = false;
    private bool right_tp_pointing = false;
    private bool cuboIsStable = false;
    
    public void TpPlayerInCubo()
    {
        // TP the player in cubo
        if (!cuboIsStable) return;
        inDeskPos.transform.position = transform.position;
        this.GetComponent<CharacterController>().enabled = false;
        transform.position = inCuboPos.transform.position;
        this.GetComponent<CharacterController>().enabled = true;            
    }

    public void TpPlayerInDesk()
    {
        // TP the player in desk
        inCuboPos.transform.position = transform.position;
        this.GetComponent<CharacterController>().enabled = false;
        transform.position = inDeskPos.transform.position;
        this.GetComponent<CharacterController>().enabled = true;
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


	void Start () 
    { 
        // Set the initial position of the desk pos
        deskPos = transform.position;

        // Set the initial position of the in_cubo_pos
        inCuboPos.transform.position = new Vector3(inCuboPos.transform.position.x, transform.position.y, inCuboPos.transform.position.z);
    }

    void Update()
    {
        
    } 
}
