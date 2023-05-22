using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableLimb : TakeDamageGeneric
{
    /// <summary>
    /// This script is placed on child objects for a BreakableJoint TakeDamage script
    /// When this gameObject takes damage it instead deals damage to the parent joint of this limb
    /// So all limbs contribute to the main joint's health
    /// The inspector variables like "health" does nothing on this script when assigning it to a gameObject.
    /// </summary>
    /// <param name="damage"></param>
    public override void TakeDamage(float damage)
    {
        // Searches in the parent hierarchy until it finds a BreakableJoint script
        // Inflicts damage on the parent joint object instead of this object since this object isn't able to die

        // Search for the relevant parent, the closest parent in the heirarchy with a BreakableJoint component
        // Make a breakableJoint that is equal to the first parent's BreakableJoint
        BreakableJoint parentJoint = GetComponentInParent<BreakableJoint>();
        while (parentJoint == null) // If that object did not have a breakableJoint component it will run this loop until it finds one
        {
            // Set the parentJoint to the parent of the previous parentJoint
            parentJoint = parentJoint.gameObject.GetComponentInParent<BreakableJoint>();
        }
        // Once it has found the closest parentJoint, call the damage function on that component
        parentJoint.TakeDamage(damage);
    }
}
