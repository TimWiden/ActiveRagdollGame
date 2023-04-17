using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageGeneric : MonoBehaviour
{
    public float health = 100f;
    public bool destroyOnDeath;

    ParticleSystemReference particleSys;
    public bool useParticleIndicators = true;
    public float particleSizeMulti = 1f;
    ParticleSystem particle1, particle2, particle3;
    bool particleSys1Active = false, particleSys2Active = false, particleSys3Active = false;
    KineticMelee hitScript;

    [HideInInspector] public float currentHealth;

    public virtual void Start()
    {
        currentHealth = health;

        hitScript = GetComponent<KineticMelee>();

        particleSys = GameObject.Find("ParticleSystemReference").GetComponent<ParticleSystemReference>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V)) Debug.Log(particleSys1Active);
    }

    public virtual void TakeDamage(float damage)
    {
        //Debug.Log(gameObject.name + " received " + damage + " damage.");

        currentHealth -= damage;

        if (currentHealth < 0) currentHealth = 0; // Make sure the health never goes below 0, otherwise it could cause problems with health-dependent calculations

        if (useParticleIndicators)
        {
            if(!particleSys1Active)
            {
                if (currentHealth < health * particleSys.sys1HealthPerc) // Checks if the health has gone below the first damage indicator level
                {
                    particle1 = Instantiate(particleSys.particleSystem1, hitScript.contactPoint, Quaternion.identity);
                    particle1.transform.rotation = Quaternion.FromToRotation(particle1.transform.up, hitScript.contactNormal);
                    // Remember the scale in a variable
                    Vector3 scale = particle1.transform.lossyScale;
                    particle1.transform.parent = this.gameObject.transform;
                    particle1.transform.localScale = scale;

                    particleSys1Active = true;
                }
            }
            if(particleSys1Active)
            {
                ParticleEffectStrength(particleSys.sys1HealthPerc, particleSys.particleSystem1, particle1); // if the particle effect is turned on then update the size of the effect according to how much health is remaining.
            }

            if (!particleSys2Active)
            {
                if (currentHealth < health * particleSys.sys2HealthPerc) // Checks if the health has gone below the first damage indicator level
                {
                    particle2 = Instantiate(particleSys.particleSystem2, hitScript.contactPoint, Quaternion.identity);
                    particle2.transform.rotation = Quaternion.FromToRotation(particle2.transform.up, hitScript.contactNormal);
                    // Remember the scale in a variable
                    Vector3 scale = particle2.transform.lossyScale;
                    particle2.transform.parent = this.gameObject.transform;
                    particle2.transform.localScale = scale;

                    particleSys2Active = true;
                }
            }
            if (particleSys2Active)
            {
                ParticleEffectStrength(particleSys.sys2HealthPerc, particleSys.particleSystem2, particle2); // if the particle effect is turned on then update the size of the effect according to how much health is remaining.
            }

            if (!particleSys3Active)
            {
                if (currentHealth < health * particleSys.sys3HealthPerc) // Checks if the health has gone below the first damage indicator level
                {
                    particle3 = Instantiate(particleSys.particleSystem3, hitScript.contactPoint, Quaternion.identity);
                    particle3.transform.rotation = Quaternion.FromToRotation(particle3.transform.up, hitScript.contactNormal);
                    // Remember the scale in a variable
                    Vector3 scale = particle3.transform.lossyScale;
                    particle3.transform.parent = this.gameObject.transform;
                    particle3.transform.localScale = scale;

                    particleSys3Active = true;
                }
            }
            if (particleSys3Active)
            {
                ParticleEffectStrength(particleSys.sys3HealthPerc, particleSys.particleSystem3, particle3); // if the particle effect is turned on then update the size of the effect according to how much health is remaining.
            }
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if(destroyOnDeath)
        {
            Destroy(gameObject);
        }

        // Disable the takedamage script
        GetComponent<TakeDamageGeneric>().enabled = false;
    }

    /*
    public void ActivateParticleCheck(float sysHealthPerc, ParticleSystem particleSystem, ParticleSystem particle)
    {
        //Debug.Log("Activating particle system");

        if(currentHealth < health * sysHealthPerc) // Checks if the health has gone below the first damage indicator level
        {
            particle = Instantiate(particleSystem, hitScript.contactPoint, Quaternion.identity);
            particle.transform.rotation = Quaternion.FromToRotation(particle.transform.up, hitScript.contactNormal);
            // Remember the scale in a variable
            Vector3 scale = particle.transform.lossyScale;
            particle.transform.parent = this.gameObject.transform;
            particle.transform.localScale = scale;
        }    
    }
    */

    void ParticleEffectStrength(float sysHealthPerc, ParticleSystem particleSystem, ParticleSystem particle)
    {
        // Change the particle system's emission rate dependent on the remaining health

        var emission = particle.emission;
        var main = particle.main;
        //var shape = particle.shape;

        // Calculate the difference from the particle's spawning health threshold and the current health
        float healthChangeMultiplier = Mathf.Clamp(health * sysHealthPerc / currentHealth, 1, particleSys.healthEffectMultiplier) * particleSizeMulti;
        //Debug.Log(healthChangeMultiplier);
        
        emission.rateOverTimeMultiplier = particleSystem.emission.rateOverTimeMultiplier / particleSys.healthEffectMultiplier * healthChangeMultiplier;
        main.startSpeedMultiplier = particleSystem.main.startSpeedMultiplier / particleSys.healthEffectMultiplier * healthChangeMultiplier;

        //Debug.Log(emission.rateOverTimeMultiplier);
        //shape.angle = initialEmissionAngle * speedMultiplier;
    }
}
