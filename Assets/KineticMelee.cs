using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ment to calculate the kintetic energy behind a hit and inflict a certain damage based off the energy.
// Does not check the angular velocity when calculating the forces
public class KineticMelee : MonoBehaviour
{
    public float damageMulti = 10, minimumImpactThreshold;

    [HideInInspector] public Vector3 lastFrameVelocity;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Kinetic energy = mv2/2
        // Movement energy = mv

        //Debug.Log(collision.impulse.magnitude);
        // Dividing the impact gives the force since (Energy = Force * Time)
        //Vector3 force = collision.impulse / Time.fixedDeltaTime;
        //Debug.Log(force.magnitude);

        //Debug.Log(gameObject.name + " " + CalculateKineticEnergy());
        //Debug.Log(collision.relativeVelocity.magnitude);





        Vector3 contactNormal = collision.GetContact(0).normal;
        Vector3 impulse = collision.impulse;

        // Both bodies see the same impulse. Flip it for one of the bodies.
        if (Vector3.Dot(contactNormal, impulse) < 0f) // if the normal facing direction is negative then inverse the impulse
            impulse *= -1f;

        Vector3 thisVelocity = rb.velocity - impulse / rb.mass;

        Vector3 colliderVelocity = Vector3.zero;

        Rigidbody colliderRb = collision.rigidbody;
        if (colliderRb != null)
        {
            colliderVelocity = colliderRb.velocity;
            if (!colliderRb.isKinematic) // If the rigidbody is able to move
                colliderVelocity += impulse / colliderRb.mass;
        }

        // Not entirely sure how Vector3.Dot works --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        // Compute how fast each one was moving along the collision normal,
        // Or zero if we were moving against the normal.
        float thisObject = Mathf.Max(0f, Vector3.Dot(thisVelocity, contactNormal));
        float otherObject = Mathf.Max(0f, Vector3.Dot(colliderVelocity, contactNormal));


        // Makes sure the damage is positive so that it doesn't end up healing instead
        float damage = Mathf.Max(0f, otherObject - thisObject);

        // If the damage is below this threshold then ignore inflicting damage
        // Basically makes it ignore light hits and scratches
        if (damage < minimumImpactThreshold) return;


        TakeDamageGeneric takeDamage = GetComponent<TakeDamageGeneric>();
        if(takeDamage != null)
        {
            Debug.Log(gameObject.name + " from " + transform.root.name + " received " + damage + " damage!");
            takeDamage.TakeDamage(damage * damageMulti);
        }
    }

    /*

    private void FixedUpdate()
    {
        lastFrameVelocity = rb.velocity;
    }

    float CalculateKineticEnergy()
    {
        float kineticEnergy = (rb.mass * Mathf.Pow(lastFrameVelocity.magnitude, 2));
        return kineticEnergy;
    }*/
}
