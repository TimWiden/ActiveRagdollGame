using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public ProjectileWeapon leftWeapon, rightWeapon;

    [SerializeField] Animator animatorCopyAnim;

    void Update()
    {
        if(Input.GetKey(KeyCode.E))
        {
            // Right side attack
            rightWeapon.Attack();
        }
        
        if(Input.GetKey(KeyCode.Q))
        {
            // Left side attack
            leftWeapon.Attack();
        }

        if (Input.GetMouseButton(0))
        {
            //Debug.Log("Attack");
            animatorCopyAnim.Play("SimpleLeftPunch");
        }

        if (Input.GetMouseButton(1))
        {
            Debug.Log("Attack 2");
            animatorCopyAnim.Play("SimpleRightPunch");
        }
    }
}
