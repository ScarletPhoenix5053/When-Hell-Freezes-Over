using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spin : MonoBehaviour
{
    public float RotationSpeed;

    private void Update()
    {
        transform.rotation =
            Quaternion.Euler(
                transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y,
                transform.rotation.eulerAngles.z + (RotationSpeed * Time.deltaTime * 10));
    }
}
