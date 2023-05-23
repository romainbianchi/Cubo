using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchBehaviour : MonoBehaviour
{
    // Flame
    private GameObject flame;

    // State
    private bool onFire = false;

    // Start is called before the first frame update
    void Start()
    {
        flame = transform.GetChild(0).gameObject;
        flame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setFire(bool fire)
    {
        flame.SetActive(fire);
        onFire = fire;
    }

    public bool isOnFire()
    {
        return onFire;
    }
}
