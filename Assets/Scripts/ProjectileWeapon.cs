using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : Weapon
{
    public GameObject projectile;
    public Transform firePoint;
    public Material color;

    public float launchForce;

    public override void Attack()
    {
        color.color = Random.ColorHSV();

        GameObject shotProjectile = Instantiate(projectile, firePoint.position, firePoint.rotation);
        // Add an initial force on the projectile
        shotProjectile.GetComponent<Rigidbody>().velocity += (firePoint.forward * launchForce);
    }
}
