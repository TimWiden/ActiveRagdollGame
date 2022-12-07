using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyLimb : MonoBehaviour
{
    public bool copyPositionToTarget, copyPosition;

    /// <summary>
    /// Copies the rotation from an animated copy onto the target rotation on ragdoll joints
    /// </summary>

    [SerializeField] Transform targetLimb;

    ConfigurableJoint configurableJoint;

    Quaternion targetInitialRotation;

    void Start()
    {
        configurableJoint = GetComponent<ConfigurableJoint>();
        targetInitialRotation = targetLimb.transform.localRotation;
    }

    void FixedUpdate()
    {
        configurableJoint.targetRotation = CopyRotation();
        if(copyPositionToTarget)
        {
            configurableJoint.targetPosition = -CopyPosition();
        }
        else if (copyPosition)
        {
            configurableJoint.gameObject.transform.position = CopyPosition();
        }
    }

    private Quaternion CopyRotation()
    {
        return Quaternion.Inverse(this.targetLimb.localRotation) * this.targetInitialRotation;
    }

    private Vector3 CopyPosition()
    {
        return targetLimb.position;
    }
}
