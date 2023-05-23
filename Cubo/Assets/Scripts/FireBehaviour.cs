using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBehaviour : MonoBehaviour
{

    // Collider
    private Collider collider;
    
    // Flames to turn off
    public GameObject flame1;
    public GameObject flame2;
    public GameObject flame3;
    public GameObject flame4;
    public GameObject flame5;
    public Collider KeyCollider;

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
    private void OnTriggerStay(Collider other)
    {   

        // Print object name
        // Debug.Log(other.name);

        if(other.GetComponent<TorchBehaviour>() != null) {
            other.GetComponent<TorchBehaviour>().setFire(true);
        }

        else if(other.GetComponent<SmallCuboBehaviour>() != null) {
            other.GetComponent<SmallCuboBehaviour>().meltIceBlock();
        }

        else if(other.name == "Cup"){
            if (other.transform.GetChild(0).GetComponent<CupBehaviour>().isWater()) {
                if (other.transform.GetChild(0).GetComponent<CupBehaviour>().isUpsideDown()){
                    other.transform.GetChild(0).GetComponent<CupBehaviour>().pourWater();
                    
                    // Deactivate collider
                    collider.enabled = false;

                    // Deactivate particles
                    GetComponent<ParticleSystem>().Stop();

                    // disable child 0
                    other.transform.GetChild(0).gameObject.SetActive(false);

                    //deactivate other flames
                    if (flame1 != null)
                    {
                        flame1.SetActive(false);
                        flame2.SetActive(false);
                        flame3.SetActive(false);
                        flame4.SetActive(false);
                        flame5.SetActive(false);

                        KeyCollider.enabled = false;
                    }
                    
                } 
            } else {
                other.transform.GetChild(0).GetComponent<CupBehaviour>().meltIce(); 
            }
        }

    }

    // Get on fire
    public bool isOnFire()
    {
        return GetComponent<ParticleSystem>().isPlaying;
    }

}
