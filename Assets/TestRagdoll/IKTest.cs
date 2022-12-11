using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour
{
    IKManager ikManager;
    public Animator animator;

    private void Start()
    {
        ikManager = GetComponent<IKManager>();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            ikManager.CopyLimbOn(false);
            animator.enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            ikManager.CopyLimbOn(true);
            animator.enabled = false;
        }
    }

}
