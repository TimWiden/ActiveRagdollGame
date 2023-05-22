using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootCopy : MonoBehaviour
{
    [SerializeField] bool isRightFoot;
    [SerializeField] LayerMask terrainLayer = default;
    [SerializeField] Transform hip = default;
    [SerializeField] IKFootCopy otherFoot = default;
    [SerializeField] float speed = 1;    
    [SerializeField] float stepDistance = 4, sideStepDistance = 2f;
    [SerializeField] float stepLength = 4;
    [SerializeField] float stepHeight = 1;
    [SerializeField] Vector3 footOffset = default;
    float footSpacing;
    Vector3 hipOld;
    Vector3 oldPosition, currentPosition, newPosition;
    Vector3 oldNormal, currentNormal, newNormal;
    Quaternion currentRotation;
    float lerp;
    Ray groundCheckRay;

    public Transform RootPos, AnimPos;
    Rigidbody rootRb;
    [SerializeField] float stepBopHeight = 1;

    private void Start()
    {
        // store the original x-spacing between the foot and the body
        footSpacing = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(hip.position.x, 0, hip.position.z));
        if (!isRightFoot) footSpacing *= -1; // Invert the foot spacing if the this is the left foot 

        // The transforms need to be defined as something so set them to the original position
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;

        rootRb = transform.root.GetComponent<Rigidbody>();

        lerp = 1;
    }

    void Update()
    {
        // Creates a reference to a new Ray. The Ray goes from a specific position towards a specified direction.
        groundCheckRay = new Ray(hip.position + (hip.right * footSpacing), Vector3.down);
        //Debug.DrawRay(hip.position + (hip.right * footSpacing), Vector3.down, Color.green, 0.1f);

        // Checks if a raycast that gets sent down hits the layermask(ground) and then sets that info to the variable 'hit'.
        if (Physics.Raycast(groundCheckRay, out RaycastHit hit, 10, terrainLayer.value))
        {

            // Checks if the distance between the foot and the hip's center is greater than the stepDistance, if it is then you need to take a new step
            // Also checks if the other foot or this foot is moving or not, if either of them are then don't
            if (Vector3.Distance(newPosition, hit.point) > stepDistance && !otherFoot.IsMoving() && !IsMoving()) // Should also add in a side step distance so that it takes steps more frequently if strafing
            {
                Debug.Log("Setting new foot placement");

                // Creates a variable that is defined based on an 'if' condition. If the ray hit position is greater than the newPosition's z-value then set the int to '1', if the statement returns false // the newPosition's z-value is greater, then return -1
                //Transform.InverseTransformPoint gets the relative local transform from "Transform" to (overload)
                // So hip.InverseTransformPoint(hit.point) returns the hit.point position relative to the hip's transform in the z-axis in local space
                // If the hit.point (the ray) is in front of the position that the foot is in currently then the direction becomes positive
                int direction = hip.InverseTransformPoint(hit.point).z > hip.InverseTransformPoint(newPosition).z ? 1 : -1;

                // Set the newPosition for the foot equal to the ray hit position plus the length of a step
                newPosition = hit.point + (hip.forward * stepLength * direction) + footOffset;
                // Set the new normal to the RaycastHit's face normal
                newNormal = hit.normal;
                
                lerp = 0;
                hipOld = hip.position;
            }
        }

        Debug.DrawLine(newPosition, hit.point, Color.yellow);

        // Move the feet by this amount for taking a new step
        if (lerp < 1)
        {
            // Moves the foot closer to the new foot position from the previous position
            Vector3 footStepPosition = Vector3.Lerp(oldPosition, newPosition, lerp);

            //Vector3 newRootPos = new Vector3(oldRootPos.x, stepBopHeight + oldRootPos.y, oldRootPos.z);
            //Vector3 rootBonePosition = Vector3.Lerp(oldRootPos, newRootPos, lerp);
            Vector3 rootBonePosition = RootPos.position;

            //Vector3 hipPos = Vector3.Lerp(hipOld, hipOld + new Vector3(0, 3, 0), lerp);
            //hip.position = hipPos;

            // Sine curve to add an arc to the step. This one controlls the height / raise for the foot during the step
            // Calculates the sinus wave by using lerp as the diameter of a circle to get circumference
            footStepPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;
            rootBonePosition.y += Mathf.Sin(lerp * Mathf.PI) * (stepHeight/4);

            // Tells the foot to move to the calculated momentary step position
            currentPosition = footStepPosition;

            RootPos.position = rootBonePosition;
            AnimPos.position = rootBonePosition;

            // Rotates the foot-transform's normal to that of the RaycastHit's normal.
            currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
            currentRotation = new Quaternion(currentNormal.x, hip.rotation.y, currentNormal.z, Quaternion.identity.w);

            // For every iteration increase the lerp variable
            lerp += Time.deltaTime * speed * rootRb.velocity.magnitude;
        }
        else // If the foot isn't supposed to move to a new position
        {
            // If the foot isn't moving then just update the previous position so that it will know once a new position has been calculated
            // Doesn't actually have to set the old stuff to the new several times since it actually just has to do it once after the new stuff has been calculated
            // But for now it just sets the old stuff to the new over and over again, might not be very optimised.
            oldPosition = newPosition;
            oldNormal = newNormal;
            //oldRootPos = rootRb.position;
        }

        // Updates the actual positions to the calculated positions
        transform.position = currentPosition;

        // Need to set the rotation of the foot to follow the ground's normal direction as well as the rotation of the mech
        transform.up = currentNormal;
        transform.rotation = currentRotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(currentPosition, 1);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(newPosition, 0.5f);
        Gizmos.DrawLine(hip.position + (hip.right * footSpacing), newPosition);
    }

    public bool IsMoving()
    {
        // Returns true if lerp is less than 1 // True if this foot is moving
        return lerp < 1;
    }
}
