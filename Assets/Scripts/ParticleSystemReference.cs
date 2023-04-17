using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemReference : MonoBehaviour
{
    public AudioClip screach;


    [Tooltip("The health change from the original health before the respective particle effect gets turned on")]
    public float sys1HealthPerc = 0.75f, sys2HealthPerc = 0.5f, sys3HealthPerc = 0.25f;

    public float healthEffectMultiplier = 3f;

    [Tooltip("System one could be sparks, system two could be smoke, system three could be fire")]
    public ParticleSystem particleSystem1, particleSystem2, particleSystem3;
}
