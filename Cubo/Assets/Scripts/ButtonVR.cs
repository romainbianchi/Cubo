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

    // Fade
    public GameObject centerEyeAnchor;
    private float fadeTime;
    private float fadeTimer = 0;
    
    // check if the player is teleporting
    private bool isTeleporting = false;

    // Check if player can press the button
    public PlayerControllerPers playerController;

    // Materials for the button
    public Material redButtonMaterial;
    public Material greenButtonMaterial;

    // Small cubo
    public GameObject smallCubo;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;

        // Set the button material to green
        button.GetComponent<Renderer>().material = greenButtonMaterial;

        // Get the fade time
        fadeTime = centerEyeAnchor.GetComponent<OVRScreenFade>().fadeTime;
    }

    void Update()
    {
        // Button is always green
        button.GetComponent<Renderer>().material = greenButtonMaterial;

        // If the player is teleporting
        if(isTeleporting){
            // start the timer and wait the end of the first fade
            fadeTimer += Time.deltaTime;
            if(fadeTimer > fadeTime){
                // Reset the timer
                fadeTimer = 0;
                // Stop teleporting
                isTeleporting = false;
                // Fade in 
                centerEyeAnchor.GetComponent<OVRScreenFade>().FadeIn();
                // TP the player in cubo
                onRelease.Invoke();
            }
        }
        
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

        // play sound
        GetComponent<AudioSource>().Play();
    }

    // On trigger exit
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == presser)
        {
            button.transform.localPosition = new Vector3(0, 0.013f, 0);
            isPressed = false;

            // only if cubo is stable
            if (!(playerController.getCuboIsStable())) return;

            isTeleporting = true;
            centerEyeAnchor.GetComponent<OVRScreenFade>().FadeOut();
        }
    }
}
