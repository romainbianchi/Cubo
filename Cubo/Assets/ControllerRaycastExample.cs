using UnityEngine;

public class ControllerRaycastExample : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public OVRInput.Button buttonToTrigger;

    void Update()
    {
        // Check if the buttonToTrigger is pressed
        if (OVRInput.Get(buttonToTrigger))
        {
            // Enable the line renderer
            lineRenderer.enabled = true;

            // Set the starting and ending points of the line
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position + transform.forward * 5f);
        }
        else
        {
            // Disable the line renderer when the button is released
            lineRenderer.enabled = false;
        }
    }
}
