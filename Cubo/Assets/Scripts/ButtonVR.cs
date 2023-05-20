using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    // Differenciate desk button from Cubo button
    public enum ButtonType { DeskButton, CuboButton }
    public ButtonType buttonType;

    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    GameObject presser;
    bool isPressed;

    // Check if player can press the button
    public PlayerControllerPers playerController;

    // Materials for the button
    public Material redButtonMaterial;
    public Material greenButtonMaterial;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;

        // Set the button material to green
        button.GetComponent<Renderer>().material = greenButtonMaterial;
    }

    void Update()
    {
        // Button is always green
        button.GetComponent<Renderer>().material = greenButtonMaterial;
        
        // Only if the button is the desk button
        if(!(buttonType == ButtonType.DeskButton)) return;
        
        // Only if cubo is not stable
        if(playerController.getCuboIsStable()) return;

        // Press gets red
        button.GetComponent<Renderer>().material = redButtonMaterial;
        
    }

    // On trigger enter
    private void OnTriggerEnter(Collider other)
    {   
        if (other.isTrigger) return;
        if (!isPressed)
        {
            button.transform.localPosition = new Vector3(0, 0.003f, 0);
            presser = other.gameObject;
            onPress.Invoke();
            isPressed = true;
        }
    }

    // On trigger exit
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.013f, 0);
            onRelease.Invoke();
            isPressed = false;
        }
    }
}
