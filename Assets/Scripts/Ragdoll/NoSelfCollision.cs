using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSelfCollision : MonoBehaviour
{
    private void Start()
    {
        Collider[] noSelfCollisionColliders = GetComponentsInChildren<Collider>();
        foreach(Collider collider in noSelfCollisionColliders)
        {
            foreach(Collider parentCollider in noSelfCollisionColliders)
            {
                // Tells all colliders in the hierarchy to ignore collisions with all of it's parents
                Physics.IgnoreCollision(collider, parentCollider, true);
            }
        }
    }
}
