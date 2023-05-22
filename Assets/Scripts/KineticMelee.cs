using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ment to calculate the kintetic energy behind a hit and inflict a certain damage based off the energy.
public class KineticMelee : MonoBehaviour
{
    public bool debug = false;

    public float damageMulti = 10, minimumImpactThreshold;

    [HideInInspector] public Vector3 lastFrameVelocity;

    Rigidbody rb;

    [HideInInspector] public Vector3 contactPoint, contactNormal;

    ParticleSystemReference PS;

    public bool useAudio = true;
    AudioSource audioSource;

    public bool useParticle = true;
    public ParticleSystem contactParticleEffect;
    private ParticleSystem particle; // The instance of the particle effect
    private float initialEmission; // variable that saves the original emission rate
    private float standstillGive = 0.1f;
    public float contactParticleMultiplier = 0.1f;
    [Range(1, 5)] public float minmaxContactParticleMultiplier = 3;
    private float initialEmissionAngle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        PS = FindObjectOfType<ParticleSystemReference>();

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1;
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



        contactPoint = collision.GetContact(0).point;

        contactNormal = collision.GetContact(0).normal;
        Vector3 impulse = collision.impulse;

        // Both bodies see the same impulse. Flip it for one of the bodies.
        if (Vector3.Dot(contactNormal, impulse) < 0f) // if the normal facing direction is negative then inverse the impulse
            impulse *= -1f;

        if (rb == null)
        {
            Debug.LogFormat("Rigidbody component missing on gameobject {0}", gameObject.name);
            rb = GetComponent<Rigidbody>();
        }

        if (rb == null)
        {
            Debug.LogFormat("Rigidbody component sitlllllllll missing on gameobject {0}", gameObject.name);
        }

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
            // Just Debugging //
            if(debug && false)
            {
                if(transform.root.gameObject != this.gameObject)
                {
                    Debug.Log(gameObject.name + " from " + transform.root.name + " received " + damage + " damage!");
                }
                else
                {
                    Debug.Log(gameObject.name + " received " + damage + " damage!");
                }
            }

            takeDamage.TakeDamage(damage * damageMulti);
        }
    }

    // Add in particle effects like sparks
    private void OnCollisionStay(Collision collision)
    {
        //Debug.Log("Staying" + gameObject.name);

        if (useParticle)
        {
            Vector3 newContactPoint = collision.GetContact(0).point;
            // Use the relative velocity to determine the particle effects scale
            Vector3 scrapingVel = collision.relativeVelocity;

            // Check if an instance of a particle effect doesn't exist, if it doesn't then instantiate a new one
            if(particle == null)
            {
                particle = Instantiate(contactParticleEffect, newContactPoint, Quaternion.identity);
                initialEmission = particle.emission.rateOverTimeMultiplier;
                initialEmissionAngle = particle.shape.angle;
            }

            var emission = particle.emission;
            var shape = particle.shape;
            if (scrapingVel.magnitude < standstillGive) // if the two touching objects aren't moving against each other then don't show the particle effect
            {
                emission.rateOverTimeMultiplier = 0;
                if(useAudio) audioSource.Stop();
            }
            else
            {
                if(useAudio)
                {
                    audioSource.clip = PS.screach;
                    if(audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
            

                float speedMultiplier = Mathf.Clamp(scrapingVel.magnitude * contactParticleMultiplier, 1 / minmaxContactParticleMultiplier, minmaxContactParticleMultiplier);
                emission.rateOverTimeMultiplier = initialEmission * speedMultiplier;
                shape.angle = initialEmissionAngle * speedMultiplier;
            }

            particle.transform.position = newContactPoint;

            // The particle effect's normal direction will be equal to that calculated direction
            // Set the particles facing direction to the angle to the contact normal
            particle.transform.LookAt(contactPoint);

            // particle.startSize

            // Now update the contact point
            // Set the old position to the current position, that way next time this function loops that position will become the previous position.
            contactPoint = newContactPoint;
        }       
    }

    private void OnCollisionExit(Collision collision)
    {
        if (useAudio) audioSource.Stop();

        if(useParticle && particle != null)
        {
            // now turn off the looping of the instantiated particle effect, that way it will destroy itself later
            var main = particle.main;
            main.loop = false;
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
