using UnityEngine;
using System.Collections;

/// <summary>
/// Takes a beating from other attacking controllers. Has gravity and knockback/up weight options? or will this be decided by the health component? TBD
/// </summary>
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(MotionController))]
public class DummyController : MonoBehaviour
{
    private MotionController mc;

    private void Awake()
    {
        mc = GetComponent<MotionController>();
    }
    private void LateUpdate()
    {
        mc.ApplyMovement();
        mc.UpdateVelocity(new Vector3(mc.XVel, 0, 0));
        mc.ApplyDrag();
    }
}
