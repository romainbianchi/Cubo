using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{

    // Collider
    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // On trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<ObjectGrabbable>().importantObject == ObjectGrabbable.ImportantObject.Torch) {
            other.GetComponent<TorchBehaviour>().setFire(true);
        }

        else if(other.GetComponent<ObjectGrabbable>().importantObject == ObjectGrabbable.ImportantObject.SmallCubo) {
            other.GetComponent<SmallCuboBehaviour>().meltIceBlock();   
        }

        else if(other.GetComponent<ObjectGrabbable>().importantObject == ObjectGrabbable.ImportantObject.Cup){
            // other.GetComponent<CupBehaviour>().meltIce();
        }

    }
}
