using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public ProjectileWeapon leftWeapon, rightWeapon;

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
    }
}
