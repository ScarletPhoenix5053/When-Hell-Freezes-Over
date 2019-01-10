using System.Collections;
using Sierra;
using UnityEngine;

public class BlobAnimator : MonoBehaviour
{
    private Animator an;
    private Vector3 currentPos;

    private void Awake()
    {
        an = GetComponent<Animator>();
    }
    private void LateUpdate()
    {
        // update position to account for animation moving the entity to an absolute space.
        //transform.position += currentPos;
    }

    public void PlayAttack()
    {
        an.SetTrigger("Leap");
    }

    private IEnumerator DontAnimatePhysicsFor(int frames)
    {
        Debug.Log("Stopping Physics anim");
        an.updateMode = AnimatorUpdateMode.Normal;
        yield return new WaitForSeconds(Utility.FramesToSeconds(frames));
    
        Debug.Log("Resuming Physics anim");
        an.updateMode = AnimatorUpdateMode.AnimatePhysics;
    }
}
