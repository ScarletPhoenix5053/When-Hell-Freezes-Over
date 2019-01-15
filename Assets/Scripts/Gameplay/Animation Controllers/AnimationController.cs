using UnityEngine;
using System.Collections;
using Sierra;

public class AnimationController : MonoBehaviour
{
    protected Animator an;
    protected IEnumerator animPhysCoroutine = null;

    protected void Awake()
    {
        an = GetComponent<Animator>();
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
