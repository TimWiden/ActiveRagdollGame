using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public ProjectileWeapon weapon;

    void Update()
    {
        if(Input.GetKey(KeyCode.X))
        {
            weapon.Attack();
        }
    }
}
