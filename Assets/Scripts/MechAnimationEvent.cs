using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Since the animator events can only call functions from scripts on the same gameobject I added this messanger script which has functions that gets called by the animations from this gameObject's animator
// Once the functions on this script gets called it instead calls the functions from the respective script from the other game objects
public class MechAnimationEvent : MonoBehaviour
{

    public BreakableJoint leftArmRootJoint, rightArmRootJoint, leftLegRootJoint, rightLegRootJoint;

    RagdollManager mechRagdollManager;


    private void Start()
    {
        // Accesses the hierarchy parent and then checks all children for the RagdollManager
        mechRagdollManager = transform.root.GetComponentInChildren<RagdollManager>();
    }

    public void ChangeJointFlex(AnimationEvent joint)
    {
        // float flex, float flexSpeed, ConfigurableJoint[] joints
        mechRagdollManager.JointFlex(joint.floatParameter / 10, joint.intParameter / 10, RootJointStringToJoint(joint.stringParameter));
        // Set the flex to '10' to reset the limbs again
        // should have an event that first has a flex higher than 1 and then one wich is 1 again
    }

    BreakableJoint RootJointStringToJoint(string input)
    {
        BreakableJoint rootJoint; // The joint group being affected currently
        switch (input)
        {
            case "leftArmJoint":
                rootJoint = leftArmRootJoint;
                break;
            case "rightArmJoint":
                rootJoint = rightArmRootJoint;
                break;
            case "leftLegJoint":
                rootJoint = leftLegRootJoint;
                break;
            case "rightLegJoint":
                rootJoint = rightLegRootJoint;
                break;
            default:
                Debug.LogError("Invalid root joint");
                return null;
        }

        return rootJoint;
    }
}
