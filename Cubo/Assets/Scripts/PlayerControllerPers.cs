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

    // Devfeloper mode
    public bool developerMode = false;
    
    public void TpPlayerInCubo()
    {
        // TP the player in cubo
        if (!cuboIsStable) return;
        this.GetComponent<CharacterController>().enabled = false;
        transform.position = inCuboPos.transform.position;
        this.GetComponent<CharacterController>().enabled = true;        
    }

    public void TpPlayerInDesk()
    {
        // TP the player in desk
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
        
    }

    void Update()
    {
        inCubo = (transform.position.x > 3.5f);

        // If cubo is not stable, we want to freeze objects with a rigidbody (only if the object position.x > 3.5f)
        if (!cuboIsStable)
        {
            Rigidbody[] objects = FindObjectsOfType<Rigidbody>();
            foreach (Rigidbody obj in objects)
            {
                if (obj.transform.position.x > 10.0f)
                {
                    // Set kinematic to true
                    obj.isKinematic = true;
                }
            }
        } 
    } 

    public void playStepSound()
    {
        // Play a step sound
        GetComponent<AudioSource>().Play();
    }
}
