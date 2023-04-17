using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // is required to easily combine two arrays into one

public class FatalHurtbox : HealthStatTracker
{
    public GameObject criticalDamageParticleSystems;

    public float criticalDamageParticleSystemPerc;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if(currentHealth < health * criticalDamageParticleSystemPerc)
        {
            criticalDamageParticleSystems.SetActive(true);
        }
    }

    public override void Die()
    {
        ConfigurableJoint[] parentJoints = GetComponentsInParent<ConfigurableJoint>();
        ConfigurableJoint[] childJoints = GetComponentsInChildren<ConfigurableJoint>();
        ConfigurableJoint[] joints = parentJoints.Union(childJoints).ToArray(); // Unites the two arrays into one

        foreach(ConfigurableJoint joint in joints)
        {
            joint.angularXDrive = new JointDrive();
            joint.angularYZDrive = new JointDrive();

            joint.GetComponent<CopyLimb>().enabled = false;
        }

        base.Die();
    }
}
