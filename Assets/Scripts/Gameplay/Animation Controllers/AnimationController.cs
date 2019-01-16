using UnityEngine;
using System.Collections;
using Sierra;

public class AnimationController : MonoBehaviour
{
    protected Animator an;
    protected IEnumerator animPhysCoroutine = null;

    protected virtual void Awake()
    {
        an = GetComponent<Animator>();
    }

    public virtual void PauseAnimation()
    {
        if (an != null) an.enabled = false;
    }
    public virtual void ResumeAnimation()
    {
        if (an != null) an.enabled = true;
    }

    protected IEnumerator AnimatePhysicsFor(int frames)
    {
        yield return GameManager.Instance.UntillHitStopInactive();
        //Debug.Log("Starting Physics anim");
        an.updateMode = AnimatorUpdateMode.AnimatePhysics;
        yield return new WaitForSeconds(Utility.FramesToSeconds(frames));
        yield return GameManager.Instance.UntillHitStopInactive();

        //Debug.Log("Stopping Physics anim");
        an.updateMode = AnimatorUpdateMode.Normal;
    }
}
