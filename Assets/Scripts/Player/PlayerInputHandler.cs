using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerActions _actions;
    private PlayerConfiguration _config;
    public PlayerInput _input;

    private bool _aimConfused = false;

    public float Movement { get; private set; }
    public float Jump { get; private set; }
    public float ThrowLimb { get; private set; }
    public Vector2 Aim { get; private set; }
    public bool FlickAiming { get; private set; } = false;
    public float LimbSwitch { get;private set; }

    private void Awake()
    {
       _actions = new PlayerActions();
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        _config = pc;
        _input = pc.Input;
        _input.onActionTriggered += MoveInput;
        _input.onActionTriggered += JumpInput;
        _input.onActionTriggered += ThrowLimbInput;
        _input.onActionTriggered += AimInput;
        _input.onActionTriggered += FlickAimInput;
        _input.onActionTriggered += SwitchLimbInput;
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
        if (_aimConfused)
        {
            Aim = -ctx.ReadValue<Vector2>();
        }
        else
        {
            Aim = ctx.ReadValue<Vector2>();
        }
    }

    public void FlickAimInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name != "SwitchAimType")
            return;
        if (FlickAiming)
        {
            FlickAiming = false;
        }
        else
        {
            FlickAiming = true;
        }
    }

    public void SwitchLimbInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name != "Switch Limb")
            return;
        LimbSwitch = ctx.ReadValue<float>();
    }

    public void MakeAimOpposite()
    {
        _aimConfused = true;
    }

    public void MakeAimNormal()
    {
        _aimConfused = false;
    }
}
