using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    public GameObject projectile;
    public Transform firePoint;
    public float damage = 10f, maxDistance = 100f, laserDuration = 0.1f;
    float laserTimer;

    RaycastHit hit;

    LineRenderer laserLine;

    private void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = false;
    }

    private void Update()
    {
        if(laserTimer < Time.time)
        {
            laserLine.enabled = false;
        }
    }

    public void Attack()
    {
        // Turn on the Line renderer
        laserLine.enabled = true;
        laserTimer = Time.time + laserDuration;

        // Set the lasers origin (vertex index 0)
        laserLine.SetPosition(0, firePoint.position);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, maxDistance))
        {
            //Debug.Log("Shot");
            //Debug.DrawLine(firePoint.position, hit.transform.position, Color.red, 0.5f);

            laserLine.SetPosition(1, hit.point);

            TakeDamageGeneric takeDamage = hit.transform.GetComponent<TakeDamageGeneric>();
            if(takeDamage != null)
            {
                takeDamage.TakeDamage(damage * 10 * Time.deltaTime);
            }
        }
        else
        {
            laserLine.SetPosition(1, firePoint.position + (firePoint.forward * maxDistance));
        }
    }
}
