using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Calls an event & launches the player backwards if they enter the trigger
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BossJumpedOnTrigger : MonoBehaviour
{
    public UnityEvent OnStomp;
    public Vector2 PlayerLaunch;

    protected PlayerController plr;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController incoming; 
        if (incoming = collision.GetComponent<PlayerController>())
        {
            plr = incoming;

            OnStomp.Invoke();
            LaunchPlayer();
        }
    }
    /// <summary>
    /// Send the player flying in a predetermined direction. Will not work if called before OnTriggerEnter2D.
    /// </summary>
    private void LaunchPlayer()
    {
        plr?.GetComponent<CharacterMotionController>().DoImpulse(PlayerLaunch);
    }
}
