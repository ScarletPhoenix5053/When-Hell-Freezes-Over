using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraChanger : MonoBehaviour
{
    /// <summary>
    /// Reference to currentley active cinemachine virtual camera.
    /// </summary>
    public CinemachineVirtualCameraBase CurrentVCam;
    /// <summary>
    /// Priorty to use when setting a cinemachine virtual camera to active.
    /// Never less than or equal to <see cref="InactivePriority"/>
    /// </summary>
    public int ActivePriority
    {
        get
        {
            return activePriority;
        }
        set
        {
            if (value <= inactivePriority)
            {
                Debug.LogError("Tried to change active priority to " + value +
                    " which is less than or equal to inactive priority: " + inactivePriority +
                    ". This is not allowed and active priority will remain at " + activePriority);
                return;
            }

            activePriority = value;
        }
    }
    /// <summary>
    /// Priorty to use when setting a cinemachine virtual camera to inactive.
    /// Never greater than or equal to <see cref="ActivePriority"/>
    /// </summary>
    public int InactivePriority
    {
        get
        {
            return inactivePriority;
        }
        set
        {
            if (value >= activePriority)
            {
                Debug.LogError("Tried to change inactive priority to " + value +
                    " which is greater than or equal to active priority: " + activePriority +
                    ". This is not allowed and active priority will remain at " + inactivePriority);
                return;
            }

            inactivePriority = value;
        }
    }

    [SerializeField]
    private int activePriority = 20;
    [SerializeField]
    private int inactivePriority = 10;

    private void Awake()
    {
        if (CurrentVCam == null)
        {
            throw new MissingReferenceException(
                "Please assign CurrentVCam to the default Cinemachine Virtual Camera before" +
                "running this script.");
        }
        else
        {
            CurrentVCam.Priority = ActivePriority;
        }
    }
    
    /// <summary>
    /// Causes a blend between two cinemachine cameras by adjusting their priority levels.
    /// </summary>
    /// <param name="newVCam">The camera to blend to.</param>
    public void ChangeTo(CinemachineVirtualCameraBase newVCam)
    {
        if (newVCam == null)
        {
            Debug.LogError("tried to change current virtual camera to null.");
            return;
        }

        // Move current camera to lower priority
        if (CurrentVCam != null)
        {
            CurrentVCam.Priority = InactivePriority;
            Debug.Log("Inactive priority is: " + InactivePriority);
        }
        else
        {
            Debug.LogWarning("Current camera reference was missing, priority will not be ajusted.");
        }

        // Move new camera to high priority
        newVCam.Priority = ActivePriority;

        Debug.Log("newCamPriority: " + newVCam.Priority);
        Debug.Log("CurrentCamPriority: " + CurrentVCam.Priority);

        // Reassign camera
        CurrentVCam = newVCam;
    }
}
