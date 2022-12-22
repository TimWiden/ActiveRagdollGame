using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollJoint : MonoBehaviour
{
    public float originalJointVal, originalMass, originalDrag;

    public ConfigurableJoint originalJoint;

    private void Awake()
    {
        originalJoint = GetComponent<ConfigurableJoint>();
        originalJointVal = originalJoint.angularXDrive.positionSpring;

        Rigidbody rb = GetComponent<Rigidbody>();
        originalMass = rb.mass;
        originalDrag = rb.drag;
    }
}
