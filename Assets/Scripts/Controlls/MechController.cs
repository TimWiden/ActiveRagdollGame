using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController : MonoBehaviour
{
    [SerializeField] Animator ragdollCopyAnim;

    public float speed = 20f, sprintSpeedMulti = 2f;
    public float strafeSpeed;
    public float jumpForce = 500f;
    static float rbForceMulti = 1f;

    public Rigidbody rb;
    public Transform movementDirection;
    public IsGrounded[] feet;
    public bool isGrounded;

    [SerializeField] IKManager ikManager;

    int horizontalDir, verticalDir;

    public float standingHeight;

    void Start()
    {
        feet = GetComponentsInChildren<IsGrounded>();

        ikManager = GetComponent<IKManager>();
    }

    void Update()
    {
        #region Movement

        // Gets the direction from the player's inputs
        horizontalDir = Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) ? 1 : !Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A) ? -1 : 0;
        verticalDir = Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) ? 1 : !Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) ? -1 : 0;

        //Vector3 moveDir = rb.transform.right * horizontalDir + rb.transform.forward * verticalDir;
        Vector3 moveDir = movementDirection.right * horizontalDir + movementDirection.forward * verticalDir;

        /*
        // If the player is giving movement inputs then turn off the copyLimb for all limbs that move via the inverse kinematics constraints, letting the ik on the ragdoll do the animating instead of the ragdolls fixed animated copy
        if (moveDir.magnitude > 0 || Input.GetKeyDown(KeyCode.F))
        {
            ikManager.CopyLimbOn(false);
            ragdollCopyAnim.enabled = true;
        }
        else
        {
            ikManager.CopyLimbOn(true);
            ragdollCopyAnim.enabled = false;
        }*/

        #region Walking

        // Checks if any of the feet are on the ground or not
        isGrounded = false; // First it says that all feet aren't grounded
        foreach (IsGrounded foot in feet)
        {
            // Now it checks if any of the feet are grounded. If it turns out any single foot is grounded then is changes isGrounded to true
            if (foot.footIsGrounded) isGrounded = true;
        }

        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.AddForce(moveDir * speed * sprintSpeedMulti * Time.deltaTime * rbForceMulti, ForceMode.VelocityChange);
            }
            else
            {
                rb.AddForce(moveDir * speed * Time.deltaTime * rbForceMulti, ForceMode.VelocityChange);
            }
        }

        #endregion

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // This time it checks if ALL feet are on the ground or not
            isGrounded = true; // First it says that all feet are grounded
            foreach (IsGrounded foot in feet)
            {
                // Now it checks if any of the feet are not grounded. If it turns out any single foot isn't grounded then is changes isGrounded to false
                if (!foot.footIsGrounded) isGrounded = false;
            }

            if (isGrounded)
            {
                rb.AddForce(rb.transform.up * jumpForce * Time.deltaTime * rbForceMulti, ForceMode.VelocityChange);
            }
        }

        #endregion
    }

    private void FixedUpdate()
    {
        Physics.Raycast(transform.position, -transform.up, standingHeight);
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawLine(transform.position,
    }
}
