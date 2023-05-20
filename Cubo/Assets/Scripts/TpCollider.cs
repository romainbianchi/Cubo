using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TpCollider : MonoBehaviour
{
    // public UnityEvent onCollision;
    // public UnityEvent offCollision;
    private Locomotion locomotion;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void setLocomotion(Locomotion locomotion)
    {
        this.locomotion = locomotion;
    }

    // On trigger enter
    private void OnTriggerStay(Collider other)
    {
        // onCollision.Invoke();
        locomotion.set_not_place_for_player(true);
    }

    // On trigger exit
    private void OnTriggerExit(Collider other)
    {
        // offCollision.Invoke();
        locomotion.set_not_place_for_player(false);

    }
}
