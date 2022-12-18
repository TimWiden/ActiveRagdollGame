using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BreakableJoint : TakeDamageGeneric
{
    public bool isLeg;
    public CopyLimb hip;
    public Rig legRig;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Die();
        }
    }

    public override void Die()
    {
        //GameObject[] limbs = GetComponentsInChildren<GameObject>();

        //Instantiate(limbs, transform.position);

        // If this joint is a leg joint then turn off the balance for the mech
        if(isLeg)
        {
            hip.copyPosition = false;
            hip.copyPositionToTarget = false;

            //ConfigurableJoint hipJoint = hip.GetComponent<ConfigurableJoint>();
            //hipJoint.angularXDrive = new JointDrive();
            //hipJoint.angularYZDrive = new JointDrive();

            // Turn off the rig's influence on the leg
            legRig.weight = 0;
        }

        Vector3 scale = transform.lossyScale;
        Debug.Log(transform.lossyScale + " + " + transform.localScale);

        // Unparent this part of the mech so it falls off
        transform.parent = null;


        Debug.Log(transform.localScale);


        ConfigurableJoint[] childJoints = GetComponentsInChildren<ConfigurableJoint>();

        foreach(ConfigurableJoint joint in childJoints)
        {
            joint.angularXDrive = new JointDrive();            
            joint.angularYZDrive = new JointDrive();

            joint.GetComponent<CopyLimb>().enabled = false;
        }    

        // Break off the limb
        ConfigurableJoint configJoint = GetComponent<ConfigurableJoint>();
        if(configJoint != null) configJoint.breakForce = 0; // prevents the error message

        transform.localScale = scale;

        base.Die();

        GetComponent<BreakableJoint>().enabled = false;
    }
}
