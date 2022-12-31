using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float healthMultiplier = 1;

    private void Awake()
    {
        foreach(TakeDamageGeneric healthComp in GetComponentsInChildren<TakeDamageGeneric>())
        {
            healthComp.health *= healthMultiplier;
        }
    }
}
