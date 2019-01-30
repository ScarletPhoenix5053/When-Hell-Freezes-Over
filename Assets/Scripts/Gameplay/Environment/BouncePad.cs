using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;

public class BouncePad : MonoBehaviour
{
    public float DelayBetweenActivations = 1f;
    public float Velocity = 20f;
    public UnityEvent OnBounce;

    protected Collider2D trigger;
    protected CharacterMotionController otherMC;
    protected SkeletonAnimation skeletonAnimation;

    protected float timeSinceLastActivation;

    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }
    private void FixedUpdate()
    {
        IncrimentActivationTimer();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If player
        if (other.tag == "Player" && 
            (InputManager.Jump()) ||
            (other.GetComponent<CharacterMotionController>().ContMotionVector.y < -1f) && !InputManager.HoldingDown())
        {
            // Apply impluse
            skeletonAnimation.AnimationState.SetAnimation(0, "Jump", false);
            skeletonAnimation.AnimationState.AddAnimation(0, "Idle", true, 0);
            otherMC = other.GetComponent<CharacterMotionController>();
            otherMC.DoImpulse(Vector2.up * Velocity);

            // Invoke event
            OnBounce.Invoke();

            // timer
            timeSinceLastActivation = 0;
        }
    }

    protected void IncrimentActivationTimer()
    {
        if (timeSinceLastActivation < DelayBetweenActivations) timeSinceLastActivation += Time.fixedDeltaTime;
    }
}