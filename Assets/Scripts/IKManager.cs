using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class IKManager : MonoBehaviour
{
    [SerializeField] CopyLimb[] limbs;

    public void CopyLimbOn(bool state)
    {
        foreach(CopyLimb limb in limbs)
        {
            limb.enabled = state;
        }
    }
}
