using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollJoint : MonoBehaviour
{
    public float originalJointSpring, originalJointMaxForce, originalMass, originalDrag;

    public ConfigurableJoint originalJoint;

    private void Awake()
    {
        originalJoint = GetComponent<ConfigurableJoint>();
        originalJointMaxForce = originalJoint.angularXDrive.maximumForce;
        originalJointSpring = originalJoint.angularXDrive.positionSpring;

        Rigidbody rb = GetComponent<Rigidbody>();
        originalMass = rb.mass;
        originalDrag = rb.drag;
    }
}
