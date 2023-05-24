using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LettersBehaviour : MonoBehaviour
{
    // CUBO letters
    public GameObject[] letters;


    // Start is called before the first frame update
    void Start()
    {
        letters = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            letters[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // C - U - B - O

        // Rotate the O continuously around its y axis
        letters[3].transform.Rotate(0, 0.5f, 0, Space.World);

        // B is blinking :
        // If HDR is on, chance to turn on the light is 10%
        // If HDR is off, chance to turn on the light is 5%
        if (letters[1].GetComponent<MeshRenderer>().material.IsKeywordEnabled("_EMISSION"))
        {
            if (Random.Range(0, 100) < 4)
            {
                letters[1].GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            }
        }
        else
        {
            if (Random.Range(0, 100) < 4)
            {
                letters[1].GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            }
        }
    }
}
