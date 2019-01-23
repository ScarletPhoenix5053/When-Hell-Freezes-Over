using UnityEngine;
using System;

public abstract class BaseController : MonoBehaviour
{
    public State CurrentState = State.Ready;
    public enum State { Ready, Action, HitStun, SuperStun, Dead }

    public virtual int Sign { get { sign = Math.Sign(transform.localScale.x); return sign; } }
    protected int sign = 1;

    protected CharacterMotionController mc;
    private AnimationController _an;

    protected virtual void Awake()
    {
        mc = GetComponent<CharacterMotionController>();
        _an = GetComponent<AnimationController>();
    }

    /// <summary>
    /// Updates <see cref="CurrentState"/>
    /// </summary>
    /// <param name="newState"></param>
    public virtual void SetState(State newState)
    {
        if (newState != CurrentState)
        {
            //if (name == "Blob") Debug.Log(name + " changed state from " + CurrentState + " to " + newState);
            CurrentState = newState;
        }
    }
    /// <summary>
    /// Perform death actions for this character.
    /// </summary>
    public abstract void Die();
}
