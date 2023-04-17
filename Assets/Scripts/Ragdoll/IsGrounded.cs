using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    [HideInInspector] public bool footIsGrounded;

    LayerMask ground;

    private void Start()
    {
        ground = LayerMask.GetMask("Ground");
    }

    private void OnTriggerEnter(Collider other)
    {
        // This was surprisingly difficult to figure out
        // The gameObject layer (collision.gameObject.layer) returns an integer same as the index of the layer (the ground layer in this project is layer 3)
        // The LayerMask on the other hand is in binary numbers, so the LayerMask for the ground layer is 8 (1000)
        // So to be able to compare the layermasks they both need to either be compared in binary or base-10
        // So to translate the base-10 number from the gameObject.layer you need to use the 'shift' operator '<<' (x << Y) which shifts the first operand by the second operator number of steps
        // So (1 << 4) would become 10000
        // So converting the gameObject.layer (3) to binary you have to do (1 << gameObject.layer) (1 << 3) which becomes (1000) or 8

        int collisionLayerBit = 1 << other.gameObject.layer;

        if(collisionLayerBit == ground.value)
        {
            footIsGrounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        int collisionLayerBit = 1 << other.gameObject.layer;

        if (collisionLayerBit == ground.value)
        {
            footIsGrounded = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = footIsGrounded ? Color.green : Color.red;

        BoxCollider collider = GetComponent<BoxCollider>();

        Gizmos.DrawCube(transform.position + collider.center, collider.size * 13);
    }
}
