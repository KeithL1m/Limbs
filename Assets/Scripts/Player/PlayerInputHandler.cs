using System;
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
        _input = pc.Input;
        _input.onActionTriggered += MoveInput;
        _input.onActionTriggered += JumpInput;
        _input.onActionTriggered += ThrowLimbInput;
        _input.onActionTriggered += AimInput;
        _input.onActionTriggered += FlickAimInput;
        _input.onActionTriggered += SwitchLimbInput;
        _input.onActionTriggered += MeleeInput;
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
        if (ctx.control.path.Contains("Mouse"))
            Aim = GetNormalizedMousePosition();
        else
        {
            if (_aimConfused)
            {
                Aim = -ctx.ReadValue<Vector2>();
                //Aim = GetNormalizedMousePosition();
            }
            else
            {
                Aim = ctx.ReadValue<Vector2>();
                //Aim = GetNormalizedMousePosition();
            }
        }
    }

    Vector2 GetNormalizedMousePosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        Vector3 playerPosition = transform.position;

        Vector2 relativeMousePosition = new Vector2(mouseWorldPosition.x - playerPosition.x, mouseWorldPosition.y - playerPosition.y);

        float maxDistance = Mathf.Max(Mathf.Abs(relativeMousePosition.x), Mathf.Abs(relativeMousePosition.y));
        if (maxDistance > 1)
        {
            relativeMousePosition /= maxDistance;
        }

        relativeMousePosition.x = Mathf.Clamp(relativeMousePosition.x, -1f, 1f);
        relativeMousePosition.y = Mathf.Clamp(relativeMousePosition.y, -1f, 1f);

        return relativeMousePosition;
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

    public void MeleeInput(InputAction.CallbackContext ctx)
    {
        if (ctx.action.name != "Melee")
            return;

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
}
