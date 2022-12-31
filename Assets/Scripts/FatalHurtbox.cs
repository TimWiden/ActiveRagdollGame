using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // is required to easily combine two arrays into one

public class FatalHurtbox : HealthStatTracker
{
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
