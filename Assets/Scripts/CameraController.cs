using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    [SerializeField] [Tooltip("How far up you can move the camera up and down")] float yAxisLookLimit = 90;
    public Transform animatedSpine;

    [SerializeField] bool hideCursor;

    float xRotation;

    private void Start()
    {
        if(hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -yAxisLookLimit, yAxisLookLimit);

        // Rotates this gameobject along it's x and y axis according to the mouse input
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        // Rotates along the up axis (y) with the mouse movement
        animatedSpine.Rotate(Vector3.up * mouseX);
    }
}
