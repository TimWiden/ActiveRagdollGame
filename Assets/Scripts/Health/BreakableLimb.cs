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
        GetComponentInParent<BreakableJoint>().TakeDamage(damage);
    }
}
