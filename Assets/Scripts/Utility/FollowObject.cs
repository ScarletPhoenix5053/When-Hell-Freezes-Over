using System;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public Transform target;
    
    void Awake()
    {
        if (target == null) throw new NullReferenceException("Please set a target!");
    }
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x, target.position.y, -10);
        }
    }
}
