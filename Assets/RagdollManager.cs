using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public float angularDriveMulti = 1, drag = 0.1f, massMulti = 1;

    [SerializeField] bool updateJoints = false;

    ConfigurableJoint[] ragdollJoints;

    public ConfigurableJoint[] leftArmJoints, rightArmJoints, leftLegJoints, rightLegJoints;

    private void Start()
    {
        ragdollJoints = GetComponentsInChildren<ConfigurableJoint>();

        foreach (ConfigurableJoint joint in ragdollJoints)
        {
            joint.gameObject.AddComponent<RagdollJoint>();
        }

        // Update the joint information
        UpdateJoints();
    }

    private void Update() // checks if the bool 'button' update joints is pressed
    {
        if (updateJoints) UpdateJoints();
    }

    // A function that temporarily increases the tention in specified joints (like when a muscle is being used compared to it being idle and relaxed)
    public void JointFlex(float flex, float flexSpeed)
    {
        float lerp = 0;

        while(lerp <= 1)
        {


            lerp += flexSpeed;
        }
    }

    void UpdateJoints()
    {
        // Reset the bool 'Button'
        updateJoints = false;

        // Access all the joints in the ragdoll hierarchy and set their default joint-drive, rigidbody drag & mass
        foreach (ConfigurableJoint joint in ragdollJoints)
        {
            // Reference to the joint's original settings that are saved
            RagdollJoint originalJoint = joint.GetComponent<RagdollJoint>();

            // You have to change the angular drive via a refering variable since you cannot directly modify it -------------------------------------------------------------------------------------------------------
            JointDrive jointDrive = new JointDrive();
            jointDrive.positionSpring = angularDriveMulti * originalJoint.originalJointVal;

            joint.angularXDrive = jointDrive;
            joint.angularYZDrive = jointDrive;


            // Rigidbody modifications
            Rigidbody jointRB = joint.GetComponent<Rigidbody>();

            jointRB.drag = originalJoint.originalDrag * drag;
            jointRB.mass = originalJoint.originalMass * massMulti;
        }
    }
}
