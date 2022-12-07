using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechController : MonoBehaviour
{
    Animator animator;

    public float speed = 100f, sprintSpeed = 4f;
    public float strafeSpeed;
    public float jumpForce = 500f;
    static float rbForceMulti = 10f;

    public Rigidbody rb;
    public IsGrounded[] feet;
    public bool isGrounded;

    IKManager ikManager;

    int horizontalDir, verticalDir;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        feet = FindObjectsOfType<IsGrounded>();

        ikManager = GetComponent<IKManager>();
    }

    void Update()
    {
        #region Movement

        // Gets the direction from the player's inputs
        horizontalDir = Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) ? 1 : !Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A) ? -1 : 0;
        verticalDir = Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) ? 1 : !Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) ? -1 : 0;

        Vector3 moveDir = rb.transform.right * horizontalDir + rb.transform.forward * verticalDir;

        // If the player is giving movement inputs then turn off the copyLimb for all limbs that move via the inverse kinematics constraints, letting the ik on the ragdoll do the animating instead of the ragdolls fixed animated copy
        if (moveDir.magnitude > 0)
        {
            Debug.Log(isGrounded);
            //ikManager.CopyLimbOn(false);
        }
        else
        {
            //ikManager.CopyLimbOn(true);
        }

        // Checks if all feet are on the ground or not
        isGrounded = true; // First it says that all feet are grounded
        foreach(IsGrounded foot in feet)
        {
            // Now it checks if any of the feet are not grounded. If it turns out any single foot isn't grounded then is changes isGrounded to false
            if (!foot.footIsGrounded) isGrounded = false;
        }

        if(isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rb.AddForce(moveDir * sprintSpeed * Time.deltaTime * rbForceMulti, ForceMode.VelocityChange);
            }
            else
            {
                rb.AddForce(moveDir * speed * Time.deltaTime * rbForceMulti, ForceMode.VelocityChange);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(rb.transform.up * jumpForce * Time.deltaTime * rbForceMulti, ForceMode.VelocityChange);
            }
        }

        #endregion

        if (Input.GetMouseButton(0))
        {
            Debug.Log("Attack");
            animator.Play("SimplePunch");
        }
    }
}