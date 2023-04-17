using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI speedometer;
    public Rigidbody body;
    public int speedometerUpdateSpeed = 5;
    int frame;

    void Update()
    {
        frame++;
        if(frame >= speedometerUpdateSpeed)
        {
            speedometer.text = Math.Round(body.velocity.magnitude, 1) + " m/s";
            frame = 0;
        }
    }
}
