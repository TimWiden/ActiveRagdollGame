using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageGeneric : MonoBehaviour
{
    public float health = 100f;
    public bool destroyOnDeath;

    [HideInInspector] public float currentHealth;

    public void Awake()
    {
        currentHealth = health;
    }

    public virtual void TakeDamage(float damage)
    {
        //Debug.Log(gameObject.name + " received " + damage + " damage.");

        currentHealth -= damage;

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
