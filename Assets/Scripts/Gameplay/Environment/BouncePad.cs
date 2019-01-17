using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float DelayBetweenActivations = 1f;
    public float Velocity = 20f;

    protected Collider2D trigger;
    protected CharacterMotionController otherMC;

    protected float timeSinceLastActivation;

    private void FixedUpdate()
    {
        IncrimentActivationTimer();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If player
        if (other.tag == "Player" && 
            (Input.GetKey(KeyCode.W) ||
            (other.GetComponent<CharacterMotionController>().ContMotionVector.y < -1f) && !Input.GetKey(KeyCode.S)))
        {
            // Apply impluse
            otherMC = other.GetComponent<CharacterMotionController>();
            otherMC.DoImpulse(Vector2.up * Velocity);

            // timer
            timeSinceLastActivation = 0;
        }
    }

    protected void IncrimentActivationTimer()
    {
        if (timeSinceLastActivation < DelayBetweenActivations) timeSinceLastActivation += Time.fixedDeltaTime;
    }
}