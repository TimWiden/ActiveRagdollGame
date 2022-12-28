using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollManager : MonoBehaviour
{
    public float angularDriveMulti = 1, dragMulti = 0.1f, massMulti = 1;

    [SerializeField] bool updateJoints = false;

    ConfigurableJoint[] ragdollJoints;

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
    public void JointFlex(float flex, float flexSpeed, BreakableJoint rootJoint)
    {
        // checks if the flex speed is set to 0 or less, if it is then the loop will continue forever
        if(flexSpeed <= 0)
        {
            Debug.LogError("Invalid float set. Flex speed set to 0 or negative. Infinite while loop would occur");
            return; // Cancel the function
        }

        float lerp = 0;

        ConfigurableJoint[] joints = rootJoint.GetComponentsInChildren<ConfigurableJoint>();

        // Calculate how much the joint is tensioned compared to the original non-tensioned values
        // Taken by calculating the original mass (original mass * multiplier)
        // The difference (delta) mass
        float oldTension = rootJoint.GetComponent<Rigidbody>().mass / (rootJoint.GetComponent<RagdollJoint>().originalMass * massMulti);

        while (lerp <= 1) // Only issue is that the exact value of the flex input is never reached. Most problematic when the joints are being reset (flex = 1) cause then the flex value always lands on something like 1.035
        {
            float currentFlex = Mathf.Lerp(oldTension, flex, lerp);

            Debug.Log("the current flex is "  + currentFlex + ". The Current lerp is " + lerp);

            // Updates all of the joints in the affected joint array to lerp between the original non-tensioned values and the max-tensioned values
            foreach (ConfigurableJoint joint in joints)
            {
                // Reference to the joint's original settings that are saved
                RagdollJoint originalJoint = joint.GetComponent<RagdollJoint>();

                // You have to change the angular drive via a refering variable since you cannot directly modify it
                JointDrive jointDrive = new();
                jointDrive.maximumForce = originalJoint.originalJointMaxForce;
                jointDrive.positionSpring = angularDriveMulti * originalJoint.originalJointSpring * currentFlex;

                //Debug.Log(originalJoint.gameObject.name + " " + (angularDriveMulti * originalJoint.originalJointSpring * currentFlex));

                joint.angularXDrive = jointDrive;
                joint.angularYZDrive = jointDrive;


                // Rigidbody modifications
                Rigidbody jointRB = joint.GetComponent<Rigidbody>();

                //jointRB.drag = originalJoint.originalDrag * dragMulti; // Don't need to alter the drag when tensing the muscle
                jointRB.mass = originalJoint.originalMass * massMulti * currentFlex;     
            }

            lerp += flexSpeed * Time.deltaTime;
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

            // You have to change the angular drive via a refering variable since you cannot directly modify it
            JointDrive jointDrive = new();
            jointDrive.maximumForce = originalJoint.originalJointMaxForce;
            jointDrive.positionSpring = angularDriveMulti * originalJoint.originalJointSpring;

            joint.angularXDrive = jointDrive;
            joint.angularYZDrive = jointDrive;


            // Rigidbody modifications
            Rigidbody jointRB = joint.GetComponent<Rigidbody>();

            jointRB.drag = originalJoint.originalDrag * dragMulti;
            jointRB.mass = originalJoint.originalMass * massMulti;
        }
    }
}
