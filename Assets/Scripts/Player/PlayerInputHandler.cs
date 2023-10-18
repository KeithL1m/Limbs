using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerActions _actions;
    private PlayerConfiguration _config;

    public float Movement { get; private set; }
    public float Jump { get; private set; }
    public float ThrowLimb { get; private set; }
    public Vector2 Aim { get; private set; }

    private void Awake()
    {
       _actions = new PlayerActions();
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        _config = pc;
        _config.Input.onActionTriggered += MoveInput;
        _config.Input.onActionTriggered += JumpInput;
        _config.Input.onActionTriggered += ThrowLimbInput;
        _config.Input.onActionTriggered += AimInput;
    }

    public void MoveInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name != "Move")
            return;
        Movement = ctx.ReadValue<float>();
    }

    public void JumpInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name != "Jump")
            return;
        Jump = ctx.ReadValue<float>();
    }

    public void ThrowLimbInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name != "ThrowLimb")
            return;
        ThrowLimb = ctx.ReadValue<float>();
    }

    public void AimInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name != "Aim")
            return;
        Aim = ctx.ReadValue<Vector2>();
    }
}
