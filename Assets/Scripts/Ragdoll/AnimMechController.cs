using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMechController : MonoBehaviour
{
    /// <summary>
    /// This animation controller is ment to work purely with root animations and not IK-constrined walking
    /// It's ment to be temporary until i figure out how to create successfull procedural walking
    /// </summary>
    
    [SerializeField] Animator anim;

    public float speed = 100f, sprintSpeed = 4f;
    public float strafeSpeed;
    public float jumpForce = 500f;
    static float rbForceMulti = 10f;

    public Rigidbody rb;
    public IsGrounded[] feet;
    public bool isGrounded;

    int horizontalDir, verticalDir;

    void Start()
    {
        //anim = GetComponentInChildren<Animator>();

        feet = FindObjectsOfType<IsGrounded>();
    }

    void FixedUpdate()
    {
        #region Movement

        // Gets the direction from the player's inputs
        horizontalDir = Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) ? 1 : !Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A) ? -1 : 0;
        verticalDir = Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) ? 1 : !Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S) ? -1 : 0;

        Vector2 MoveDir = new Vector2(horizontalDir, verticalDir);
        Debug.Log(MoveDir.magnitude);
        //Vector3 moveDir = rb.transform.right * horizontalDir + rb.transform.forward * verticalDir;

        // Checks if all feet are on the ground or not
        isGrounded = true; // First it says that all feet are grounded
        foreach (IsGrounded foot in feet)
        {
            // Now it checks if any of the feet are not grounded. If it turns out any single foot isn't grounded then is changes isGrounded to false
            if (!foot.footIsGrounded) isGrounded = false;
        }

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(rb.transform.up * jumpForce * Time.deltaTime * rbForceMulti, ForceMode.VelocityChange);
            }
        }

        if (MoveDir.magnitude != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("Run", true);
                //rb.AddForce(moveDir * sprintSpeed * Time.deltaTime * rbForceMulti, ForceMode.VelocityChange);
            }
            else
            {
                anim.SetBool("Walk", true);
                //rb.AddForce(moveDir * speed * Time.deltaTime * rbForceMulti, ForceMode.VelocityChange);
            }
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
        }

        #endregion

        if (Input.GetMouseButton(0))
        {
            Debug.Log("Attack");
            anim.Play("SimplePunch");
        }
    }
}
