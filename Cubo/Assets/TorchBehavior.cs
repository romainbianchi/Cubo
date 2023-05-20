using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchBehavior : MonoBehaviour
{
    public GameObject Ice;
    public GameObject IceCollider;
    public GameObject smallCubo;
    public GameObject meltedIce;

    private Collider collider;

    private Vector3 icePosition;

    // Start is called before the first frame update
    void Start()
    {
        collider = smallCubo.GetComponent<Collider>();
        meltedIce.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // on trigger enter
    private void OnTriggerEnter(Collider other)
    {
        if (other == collider){
            Destroy(Ice);
            Destroy(IceCollider);
            meltedIce.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
