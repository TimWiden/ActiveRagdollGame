using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadline : MonoBehaviour
{
    string[] names = { "Astrid", "Abdi", "Otis", "Martin" };

    int ACounter = 0;

    void Start()
    {
        int f = 0;
        do
        {
            Debug.Log("Done");
        }
        while (f < 5);
        {
            Debug.Log("Done " + f);
            f++;
        }

        for(int i = 0; i < names.Length; i++)
        {
            if (names[i].Contains("A") || names[i].Contains("a"))
            {
                Debug.Log("The name " + names[i] + " contains 'A'");
                ACounter++;
            }
        }
    }
    private void Update()
    {
        while (ACounter > 0)
        {
            Debug.Log("And another 'A'");
            ACounter--;
        }
    }
}
