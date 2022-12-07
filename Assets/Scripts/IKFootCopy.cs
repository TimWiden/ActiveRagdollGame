using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootCopy : MonoBehaviour
{
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform body = default;
    [SerializeField] IKFootCopy otherFoot = default;
    [SerializeField] float speed = 1;
    [SerializeField] float stepDistance = 4;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;
    float footSpacing;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    float lerp;
    Ray groundCheckRay;

    private void Start()
    {
        // store the original x-spacing between the foot and the body
        footSpacing = transform.position.x - body.position.x; 

        // The transforms need to be defined as something so set them to the original position
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;

        lerp = 1;
    }

    void Update()
    {
        // Creates a reference to a new Ray. The Ray goes from a specific position towards a specified direction.
        groundCheckRay = new Ray(body.position + new Vector3(footSpacing, 0, 0), Vector3.down); // (body.right * footSpacing)

        // Checks if a raycast that gets sent down hits the layermask(ground) and then sets that info to the variable 'hit'.
        if (Physics.Raycast(groundCheckRay, out RaycastHit hit, 10, terrainLayer.value))
        {
            // Checks if the distance between the ------- is greater than the stepDistance, if it is then you need to take a new step
            // Also checks if the other foot or this foot is moving or not, if either of them are then don't
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.IsMoving() && !IsMoving())
            {
                // Creates a variable that is defined based on an 'if' condition. If the ray hit position is greater than the newPosition's z-value then set the int to '1', if the statement returns false // the newPosition's z-value is greater, then return -1
                int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;

                // Set the newPosition to the ray hit position // ------------------------------------------------------------------------------------------------------------------------------------------------------ Not sure entirely why the stepLength is required
                newPosition = hit.point + (body.forward * stepLength * direction) + footOffset;
                // Set the new normal to the RaycastHit's face normal
                newNormal = hit.normal;
                
                lerp = 0;
            }
        }

        if (lerp < 1)
        {
            // Moves the foot closer to the new foot position
            Vector3 footStepPosition = Vector3.Lerp(oldPosition, newPosition, lerp);

            // Sine curve to add an arc to the step. This one controlls the height / raise for the foot during the step
            // Calculates the sinus wave by using lerp as the diameter of a circle to get circumference
            footStepPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            // Tells the foot to move to the calculated momentary step position
            currentPosition = footStepPosition;

            // Rotates the foot-transform's normal to that of the RaycastHit's normal.
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);

            // For every iteration increase the lerp variable
            lerp += Time.deltaTime * speed;
        }
        else // If the foot isn't supposed to move to a new position
        {
            //Debug.Log("old and new position " + oldPosition.ToString() + newPosition.ToString());
            oldPosition = newPosition;
            oldNormal = newNormal;
        }

        // Updates the actual positions to the calculated positions
        transform.position = currentPosition;
        transform.up = currentNormal;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
        Gizmos.DrawLine(body.position + (body.right * footSpacing), newPosition);
    }

    public bool IsMoving()
    {
        // Returns true if lerp is less than 1 // True if this foot is moving
        return lerp < 1;
    }
}
