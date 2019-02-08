using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [HideInInspector]
    public static InputManager Instance;
    private const float booleanDeadZone = 0.3f;

    #region KeyCode Refrences
    [SerializeField]
    private KeyCode ControllerAttackButton = KeyCode.JoystickButton2;
    [SerializeField]
    private KeyCode KeyboardAttackButton = KeyCode.J;
    [SerializeField]
    private KeyCode ControllerSpecialAttackButton = KeyCode.JoystickButton3;
    [SerializeField]
    private KeyCode KeyboardSpecialAttackButton = KeyCode.K;
    [SerializeField]
    private KeyCode ControllerRangedAttackButton = KeyCode.JoystickButton5;
    [SerializeField]
    private KeyCode KeyboardRangedAttackButton = KeyCode.L;
    [SerializeField]
    private KeyCode ControllerRollButton = KeyCode.JoystickButton1;
    [SerializeField]
    private KeyCode KeyboardRollButton = KeyCode.LeftShift;
    [SerializeField]
    private KeyCode ControllerJumpButton = KeyCode.JoystickButton0;
    [SerializeField]
    private KeyCode KeyboardJumpButton = KeyCode.Space;

    [SerializeField]
    private KeyCode ControllerInteractButton = KeyCode.JoystickButton2;
    [SerializeField]
    private KeyCode KeyboardInteractButton = KeyCode.E;

    [SerializeField]
    private KeyCode ControllerPauseButton = KeyCode.JoystickButton7;
    [SerializeField]
    private KeyCode KeyboardPauseButton = KeyCode.Escape;
    #endregion

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    #region Button Inputs
    public static bool Attack()
    {
        return Input.GetKeyDown(Instance.ControllerAttackButton)
            || Input.GetKeyDown(Instance.KeyboardAttackButton);
    }
    public static bool SpecialAttack()
    {
        return Input.GetKeyDown(Instance.ControllerSpecialAttackButton)
            || Input.GetKeyDown(Instance.KeyboardSpecialAttackButton);
    }
    public static bool RangedAttack()
    {
        return Input.GetKeyDown(Instance.ControllerRangedAttackButton)
            || Input.GetKeyDown(Instance.KeyboardRangedAttackButton);
    }
    public static bool RangedAttackHeld()
    {
        throw new NotImplementedException();
    }
    public static bool Jump()
    {
        return Input.GetKeyDown(Instance.ControllerJumpButton)
            || Input.GetKeyDown(Instance.KeyboardJumpButton);
    }
    public static bool JumpHeld()
    {
        return Input.GetKey(Instance.ControllerJumpButton)
            || Input.GetKey(Instance.KeyboardJumpButton);
    }
    public static bool Roll()
    {
        return Input.GetKeyDown(Instance.ControllerRollButton)
            || Input.GetKeyDown(Instance.KeyboardRollButton);
    }
    public static bool Interact()
    {
        return Input.GetKeyDown(Instance.ControllerInteractButton)
            || Input.GetKeyDown(Instance.KeyboardInteractButton);
    }
    public static bool Pause()
    {
        return Input.GetKeyDown(Instance.ControllerPauseButton)
            || Input.GetKeyDown(Instance.KeyboardPauseButton);
    }


    public static bool HoldingDown()
    {
        return VeticalAxis() < -booleanDeadZone;
    }
    public static bool HoldingUp()
    {
        return VeticalAxis() > booleanDeadZone;
    }
    #endregion
    #region Axial Inputs
    public static float MotionAxis()
    {
        var motionAxis = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("Horizontal") < -booleanDeadZone) motionAxis -= 1;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.GetAxis("Horizontal") > booleanDeadZone) motionAxis += 1;
        return motionAxis;
    }
    public static float VeticalAxis()
    {
        var motionAxis = 0f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("Vertical") < -booleanDeadZone) motionAxis -= 1;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Vertical") > booleanDeadZone) motionAxis += 1;
        return motionAxis;
    }
    #endregion
}
