using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerActions _actions;
    private PlayerConfiguration _config;
    private PlayerActions _inputActions;

    #region InputActions
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _throwLimbAction;
    private InputAction _aimAction;
    private InputAction _flickAimAction;
    private InputAction _switchLimbAction;
    private InputAction _meleeAction;
    #endregion

    private bool _aimConfused = false;

    public float Movement { get; private set; }
    public float Jump { get; private set; }
    public float ThrowLimb { get; private set; }
    public Vector2 Aim { get; private set; }
    public bool FlickAiming { get; private set; } = false;
    public float LimbSwitch { get; private set; }
    public float Melee { get; private set; }

    public event Action<float> MeleeAttack;

    private void Awake()
    {
        _actions = new PlayerActions();
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        _config = pc;
        _inputActions = new PlayerActions();

        _moveAction = _inputActions.Gameplay.Move;
        _moveAction.performed += MoveInput;
        _moveAction.Enable();

        _jumpAction = _inputActions.Gameplay.Jump;
        _jumpAction.performed += JumpInput;
        _jumpAction.Enable();

        _throwLimbAction = _inputActions.Gameplay.ThrowLimb;
        _throwLimbAction.performed += ThrowLimbInput;
        _throwLimbAction.Enable();

        _aimAction = _inputActions.Gameplay.Aim;
        _aimAction.performed += AimInput;
        _aimAction.Enable();

        _flickAimAction = _inputActions.Gameplay.SwitchAimType;
        _flickAimAction.performed += FlickAimInput;
        _flickAimAction.Enable();

        _switchLimbAction = _inputActions.Gameplay.SwitchLimb;
        _switchLimbAction.performed += SwitchLimbInput;
        _switchLimbAction.Enable();

        _meleeAction = _inputActions.Gameplay.Melee;
        _meleeAction.performed += MeleeInput;
        _meleeAction.Enable();
    }

    public void MoveInput(InputAction.CallbackContext ctx)
    {
        if (_config.Device != ctx.control.device)
        {
            return;
        }
        Movement = ctx.ReadValue<float>();
    }

    public void JumpInput(InputAction.CallbackContext ctx)
    {
        if (_config.Device != ctx.control.device)
        {
            return;
        }
        Jump = ctx.ReadValue<float>();
    }

    public void ThrowLimbInput(InputAction.CallbackContext ctx)
    {
        if (_config.Device != ctx.control.device)
        {
            return;
        }
        ThrowLimb = ctx.ReadValue<float>();
    }

    public void AimInput(InputAction.CallbackContext ctx)
    {
        if (_config.Device != ctx.control.device)
        {
            return;
        }
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
        if (_config.Device != ctx.control.device)
        {
            return;
        }
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
        if (_config.Device != ctx.control.device)
        {
            return;
        }
        LimbSwitch = ctx.ReadValue<float>();
    }

    public void MeleeInput(InputAction.CallbackContext ctx)
    {
        if (_config.Device != ctx.control.device)
        {
            return;
        }
        if (ctx.phase == InputActionPhase.Started)
        {
            Melee = ctx.ReadValue<float>();
            MeleeAttack?.Invoke(Melee);
        }
    }

    public void MakeAimOpposite()
    {
        _aimConfused = true;
    }

    public void MakeAimNormal()
    {
        _aimConfused = false;
    }

    private void OnDestroy()
    {
        _moveAction.performed -= MoveInput;
        _moveAction.Disable();

        _jumpAction.performed -= JumpInput;
        _jumpAction.Disable();

        _throwLimbAction.performed -= ThrowLimbInput;
        _throwLimbAction.Disable();

        _aimAction.performed -= AimInput;
        _aimAction.Disable();

        _flickAimAction.performed -= FlickAimInput;
        _flickAimAction.Disable();

        _switchLimbAction.performed -= SwitchLimbInput;
        _switchLimbAction.Disable();

        _meleeAction.performed -= MeleeInput;
        _meleeAction.Disable();
    }
}
