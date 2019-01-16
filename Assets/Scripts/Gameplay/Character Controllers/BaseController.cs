using UnityEngine;
using System.Collections;

public abstract class BaseController : MonoBehaviour
{
    public State CurrentState = State.Ready;
    public enum State { Ready, InAction, InHitstun, Dead }

    protected CharacterMotionController mc;

    protected virtual void Awake()
    {
        mc = GetComponent<CharacterMotionController>();
    }

    /// <summary>
    /// Updates <see cref="CurrentState"/>
    /// </summary>
    /// <param name="newState"></param>
    public virtual void SetState(State newState)
    {
        if (newState != CurrentState)
        {
            if (name == "Blob") Debug.Log(name + " changed state from " + CurrentState + " to " + newState);
            CurrentState = newState;
        }
    }
    /// <summary>
    /// Perform death actions for this character.
    /// </summary>
    public abstract void Die();
}
