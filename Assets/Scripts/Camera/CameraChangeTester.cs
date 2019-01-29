using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChangeTester : MonoBehaviour
{
    public CameraChanger CameraChanger;
    public CinemachineVirtualCameraBase[] VCams;

    bool alternate;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (alternate) CameraChanger.ChangeTo(VCams[0]);
            else CameraChanger.ChangeTo(VCams[1]);

            alternate = !alternate;
        }
    }
}
