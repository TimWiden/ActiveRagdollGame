using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Weapon leftWeapon, rightWeapon;

    [SerializeField] Animator animatorCopyAnim;

    void Update()
    {
        // If this is not the local player game object then don't take input
        if (PlayerManagerCS.LocalPlayerInstance != gameObject)
        {
            return;
        }


        if (Input.GetKey(KeyCode.E))
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
            //Debug.Log("Attack 2");
            animatorCopyAnim.Play("SimpleRightPunch");
        }
    }
}
