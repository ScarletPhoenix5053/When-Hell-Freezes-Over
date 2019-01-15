using UnityEngine;
using System.Collections;

public abstract class BaseController : MonoBehaviour
{
    public State CurrentState = State.Ready;
    public enum State { Ready, InAction, InHitstun, Dead }

    protected MotionController mc;

    protected virtual void Awake()
    {
        mc = GetComponent<MotionController>();
    }

    /// <summary>
    /// Updates <see cref="CurrentState"/>
    /// </summary>
    /// <param name="newState"></param>
    public virtual void SetState(State newState)
    {
        if (newState != CurrentState)
        {
            CurrentState = newState;
            //Debug.Log(name + " changed state from " + CurrentState + " to " + newState);
        }
    }
}
