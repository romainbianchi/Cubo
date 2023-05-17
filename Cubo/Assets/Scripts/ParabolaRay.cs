using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaRay : MonoBehaviour
{
    //Point de contact avec le sol
    public RaycastHit hit;

    //Raycast parameters
    private List<Vector3> positions;
    private LineRenderer lineRenderer;
    private Vector3 raycastDirection;
    private Vector3 raycastPosition;

   
    //Settings
    public int rayCastSpawnLimit = 20;
    public float rayCastLength = 0.1f;
    public float gravity = 9.81f;
    public float smooth = 0.1f;
    private int amountOfRaycastsSpawned;
 
    //Ball
    private GameObject ball;

    private void Start(){
        lineRenderer = GetComponent<LineRenderer>(); 

        // Create a ball
        ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);   // cree la sphere
        ball.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // change la taille de la sphere
        ball.GetComponent<Renderer>().material.color = Color.red;  // change la couleur de la sphere
        Destroy(ball.GetComponent<SphereCollider>());              // enleve le collider de la sphere
        
    }

    private void Update()
    {
        // Ici on obtient la liste de points de la parabole (et on lance le raycast a l'intérieur de la fonction)
        Vector3[] curvePoints = curvedRaycast();

        // Ici on update les parametre du line renderer en fonction de la liste de points
        int numPoints = curvePoints.Length;
        lineRenderer.positionCount = numPoints;

        // Ici on update les positions du line renderer en fonction de la liste de points
        for (int i = 0; i < numPoints; i++)
        {
            Vector3 point = curvePoints[i];
            lineRenderer.SetPosition(i, point);
        }

        // Display a ball at the hit point
        ball.transform.position = hit.point;
    }
 
    private Vector3[] curvedRaycast()
    {
        amountOfRaycastsSpawned = 0;
        positions = new List<Vector3>();
        raycastPosition = transform.position;
        raycastDirection = transform.forward;
        
        // Tant que le nombre de raycast est inferieur au nombre de raycast max on continue a creer la parabole
        while (amountOfRaycastsSpawned < rayCastSpawnLimit)
        {
            positions.Add(raycastPosition);

            // Si le raycast touche un objet, on arrete le raycast
            if (Physics.Raycast(raycastPosition, raycastDirection, out hit, 2*rayCastLength))
            {
                positions.Add(hit.point);
                break;
            }
            
            // Sinon on append le raycast
            raycastPosition += raycastDirection * rayCastLength;
            raycastDirection += new Vector3(0.0f, -gravity * smooth, 0.0f);
            
            amountOfRaycastsSpawned++;
        }
        
        // On retourne la liste de points pour afficher le line renderer apres l'appel de la fonction
        return positions.ToArray();
    }
}