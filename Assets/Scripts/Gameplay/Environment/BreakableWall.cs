using UnityEngine;
using UnityEngine.Events;
using System;
using Sierra.Combat2D;

/// <summary>
/// Interacts with <see cref="Hitbox"/>es to for hit detection.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class BreakableWall : Hurtbox
{
    [Serializable]
    public class BreakableWallEvents
    {
        public UnityEvent OnFailedHit;
        public UnityEvent OnBreak;
    }
    public BreakableWallEvents Events;
    public int Strength = 1;

    private void Awake()
    {
        Collider = GetComponent<Collider2D>();
    }

    public override bool CheckHit()
    {
        Debug.LogError("Called the wrong CheckHit Method on Breakable wall: " + name);
        return false;
    }
    public bool CheckHit(int strength)
    {
        // If weapon strength too weak
        if (strength < Strength)
        {
            // Failed Hit
            Events.OnFailedHit.Invoke();

            Debug.Log(name + " needs to be hit with a stronger weapon");
            return false;
        }
        else
        {
            // Break
            Events.OnBreak.Invoke();
            SetState(State.Inactive);
            gameObject.SetActive(false);

            Debug.Log(name + " broke");
            return true;
        }
    }
}
