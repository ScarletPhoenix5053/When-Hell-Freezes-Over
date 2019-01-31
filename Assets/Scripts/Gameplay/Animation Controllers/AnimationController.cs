using UnityEngine;
using System.Collections;
using System;
using Sierra;
using Spine.Unity;
using Spine;

public class AnimationController : MonoBehaviour
{
    /// <summary>
    /// Returns the <see cref="TrackEntry"/> of the first track in spine animation. Null if nothing is playing.
    /// </summary>
    public TrackEntry CurrentTrack { get { return sk_an.AnimationState.GetCurrent(0); } }

    protected Animator an;
    protected SkeletonAnimation sk_an;
    protected IEnumerator animPhysCoroutine = null;

    protected virtual void Awake()
    {
        an = GetComponent<Animator>();
        sk_an = GetComponent<SkeletonAnimation>();
    }

    public virtual void SetSkin(string skinName)
    {
        sk_an.skeleton.SetSkin(skinName);
    }
    public virtual void PauseAnimation()
    {
        if (an != null) an.enabled = false;
    }
    public virtual void ResumeAnimation()
    {
        if (an != null) an.enabled = true;
    }
    /// <summary>
    /// Sets X orientation to match sign. Automatically processes sign.
    /// </summary>
    /// <param name="sign"></param>
    public void OrientTo(int sign)
    {
        sign = Math.Sign(sign);
        sk_an.Skeleton.ScaleX = sign;
    }
    protected IEnumerator AnimatePhysicsFor(int frames)
    {
        //Debug.Log("Starting Physics anim");
        an.updateMode = AnimatorUpdateMode.AnimatePhysics;
        yield return new WaitForSeconds(Utility.FramesToSeconds(frames));

        //Debug.Log("Stopping Physics anim");
        an.updateMode = AnimatorUpdateMode.Normal;
    }
}
