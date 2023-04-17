using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    Rigidbody gunRb;

    public GameObject projectile;
    public Transform firePoint;

    public float projectileForce, recoil, fireRate;
    float fireTimestamp;

    private void Start()
    {
        gunRb = transform.parent.GetComponent<Rigidbody>();
    }

    public override void Attack()
    { 
        //if()

        GameObject shotProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        // Add an initial force on the projectile
        shotProjectile.GetComponent<Rigidbody>().velocity += (firePoint.forward * projectileForce);

        if(recoil != 0)
        {
            gunRb.AddForce(-firePoint.forward * recoil, ForceMode.VelocityChange);
        }

        fireTimestamp = Time.time;
    }
}
