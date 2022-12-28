using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ment to calculate the kintetic energy behind a hit and inflict a certain damage based off the energy.
public class KineticMelee : MonoBehaviour
{
    public float damageMulti;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.impulse.magnitude);
    }
}
