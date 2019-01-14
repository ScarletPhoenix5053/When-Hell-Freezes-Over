using System.Collections;
using Sierra;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator an;
    private IEnumerator animPhysCoroutine = null;

    private void Awake()
    {
        an = GetComponent<UnityEngine.Animator>();
    }

    public void PlayAttack()
    {
        an.SetTrigger("Leap");
        if (animPhysCoroutine != null) StopCoroutine(animPhysCoroutine);
        animPhysCoroutine = AnimatePhysicsFor(100);
        StartCoroutine(animPhysCoroutine);
    }

    private IEnumerator AnimatePhysicsFor(int frames)
    {
        //Debug.Log("Starting Physics anim");
        an.updateMode = AnimatorUpdateMode.AnimatePhysics;
        yield return new WaitForSeconds(Utility.FramesToSeconds(frames));
    
        //Debug.Log("Stopping Physics anim");
        an.updateMode = AnimatorUpdateMode.Normal;
    }
}
