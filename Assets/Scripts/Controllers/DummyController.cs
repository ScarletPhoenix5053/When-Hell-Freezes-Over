using UnityEngine;
using System;

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
        throw new NotImplementedException();
    }
}
