using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Deprecated
public class IKFootSolver : MonoBehaviour
{
    public Transform body;

    [Tooltip("Distance between the foot and the body's center")] public float footSpacing = 2f;
    [Tooltip("Maximum allowed distance between feet placement, once the feet are further away than this distance then take a new step")] public float stepDistance = 2f;
    [Tooltip("How high the foot is lifted when taking a new step")] public float stepHeight = 1f;
    [Tooltip("Stepping speed")] public float speed = 1f;

    public LayerMask groundLayer;

    Ray groundCheck;
    float lerp;
    Vector3 currentPosition, newPosition, oldPosition;

    private void Start()
    {
        Debug.Log(transform.position);
        currentPosition = transform.position;
        Debug.Log(currentPosition);
    }

    void Update()
    {
        transform.position = currentPosition;

        // Creates a reference to a new Ray. The Ray goes from a specific position towards a specified direction.
        Ray groundCheck = new Ray(body.position + (body.right * footSpacing), Vector3.down);

        //if (Physics.Raycast((body.position + (body.right * footSpacing), Vector3.down, 10, terrainLayer.value))
        // Checks if a raycast that gets sent down hits the layermask(ground) and then sets that info to the variable 'hit'.
        if (Physics.Raycast(groundCheck, out RaycastHit hit, 5, groundLayer.value))
        {
            // Checks if the distance between the ------- is greater than the stepDistance, if it is then you need to take a new step
            if(Vector3.Distance(newPosition, hit.point) > stepDistance)
            {
                lerp = 0;

                newPosition = hit.point;
            }
        }

        if (lerp < 1)
        {
            // Moves the foot closer to the new foot position
            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);

            // Sine curve to add an arc to the step. This one controlls the height for the foot during the step
            footPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            // Now changes the foot's position to the calculated position
            currentPosition = footPosition;

            //
            lerp += Time.deltaTime * speed;
        }
        else
        {
            // If the foot doesn't have to move then continue to keep the foot in the last known spot.
            oldPosition = newPosition;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.05f);
        Gizmos.DrawRay(groundCheck);
    }
}
