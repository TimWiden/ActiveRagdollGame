using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosive : MonoBehaviour
{
    public float explodeDelay;

    public bool destroyOnBlow = true;

    public ParticleSystem explosionParticles;

    private void Start()
    {
        if(explodeDelay != 0)
        {
            StartCoroutine(Fuse());
        }
    }

    IEnumerator Fuse()
    {
        yield return new WaitForSeconds(explodeDelay);
        Explode();
    }

    void Explode()
    {
        GetComponent<Collider>().enabled = false; // disable the collider so that the particle system doesn't interact with this gameObject
        // Would be better to use Physics.IgnoreCollision but it doens't work with the particle systems collider

        ParticleSystem particle = Instantiate(explosionParticles, transform.position, Quaternion.identity);

        if(destroyOnBlow)
        {
            Destroy(gameObject);
        }
    }
}
