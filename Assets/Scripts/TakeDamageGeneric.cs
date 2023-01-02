using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageGeneric : MonoBehaviour
{
    public float health = 100f;
    public bool destroyOnDeath;

    ParticleSystemReference particleSys;
    public bool useParticleIndicators;
    bool sys1Active, sys2Active, sys3Active; // bools that check if the respective particle systems are active currently or not
    KineticMelee hitScript;

    [HideInInspector] public float currentHealth;

    public virtual void Start()
    {
        currentHealth = health;

        hitScript = GetComponent<KineticMelee>();

        particleSys = GameObject.Find("ParticleSystemReference").GetComponent<ParticleSystemReference>();
    }

    public virtual void TakeDamage(float damage)
    {
        //Debug.Log(gameObject.name + " received " + damage + " damage.");

        currentHealth -= damage;

        if(useParticleIndicators)
        {
            if (currentHealth < health * particleSys.sys1HealthPerc && !sys1Active) // Checks if the health has gone below the first damage indicator level
            {
                sys1Active = true;

                ParticleSystem particle1 = Instantiate(particleSys.particleSystem1, hitScript.contactPoint, Quaternion.identity);
                particle1.transform.rotation = Quaternion.FromToRotation(particle1.transform.up, hitScript.contactNormal);
                // Remember the scale in a variable
                Vector3 scale = particle1.transform.lossyScale;
                particle1.transform.parent = this.gameObject.transform;
                particle1.transform.localScale = scale;
            }

            if (currentHealth < health * particleSys.sys2HealthPerc && !sys2Active) // Checks if the health has gone below the first damage indicator level
            {
                sys2Active = true;
                ParticleSystem particle2 = Instantiate(particleSys.particleSystem2, hitScript.contactPoint, Quaternion.identity);
                particle2.transform.rotation = Quaternion.FromToRotation(particle2.transform.up, hitScript.contactNormal);
                // Remember the scale in a variable
                Vector3 scale = particle2.transform.lossyScale;
                particle2.transform.parent = this.gameObject.transform;
                particle2.transform.localScale = scale;
            }

            if (currentHealth < health * particleSys.sys3HealthPerc && !sys3Active) // Checks if the health has gone below the first damage indicator level
            {
                sys3Active = true;
                ParticleSystem particle3 = Instantiate(particleSys.particleSystem3, hitScript.contactPoint, Quaternion.identity);
                particle3.transform.rotation = Quaternion.FromToRotation(particle3.transform.up, hitScript.contactNormal);
                // Remember the scale in a variable
                Vector3 scale = particle3.transform.lossyScale;
                particle3.transform.parent = this.gameObject.transform;
                particle3.transform.localScale = scale;
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
}
