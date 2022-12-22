using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This is ment for the body parts that show up on the health state HUD element
// Updates the HUD according to this body part's current state
public class HealthStatTracker : TakeDamageGeneric
{
    public Image bodyPart;
    [Range(0, 1)] public float statAlpha = 0.75f, deathAlpha = 0.5f;

    private void Start()
    {
        // Resets the color of the HUD element body part to green
        bodyPart.color = new Color(0, 255, bodyPart.color.b);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        // if the body part isn't already dead
        if(bodyPart.color != new Color(0, 0, 0, deathAlpha))
        {
            //Debug.Log(currentHealth);

            // Calculate the remaining health (1-0) (percent/100)
            float healthChange = Mathf.Clamp(currentHealth / health, 0, 1);
            // The color is 0-1 not 16-bit
            // Fucked me over for a while...
            Color healthColor = new Color(1 - healthChange, healthChange, 0, statAlpha);
            bodyPart.color = healthColor;
        }
    }

    public override void Die()
    {
        // Set the HUD element to black
        bodyPart.color = new Color(0, 0, 0, deathAlpha);

        base.Die();
    }
}
