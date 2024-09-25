using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuNavegation : MonoBehaviour
{
    [SerializeField] private Button _referenceButton;
    private GameObject _currentlySelectedButton;

    public InputDevice Device { get; set; }

    private PlayerActions _actions;
    private PlayerConfiguration _config;
    private PlayerActions _inputActions;

    private InputAction _joystickAction;

    [SerializeField] private float _moveSelectionTime = 0.2f;
    private float _coolDownTimer = 0.0f;
    private bool _inCoolDown = false;

    void Awake()
    {
        _currentlySelectedButton = _referenceButton.gameObject;
        _inputActions = new PlayerActions();

        _joystickAction = _inputActions.MenuNav.Move;
        _joystickAction.performed += JoystickInput;
        _joystickAction.Enable();
    }

    private void Update()
    {
        if(_coolDownTimer >= 0.0f && _inCoolDown)
        {
            _coolDownTimer -= Time.deltaTime;
            if(_coolDownTimer <= 0.0f)
            {
                _inCoolDown = false;
                _coolDownTimer = _moveSelectionTime;
            }
        }
    }

    private void OnDestroy()
    {
        _joystickAction.performed -= JoystickInput;
        _joystickAction.Disable();
    }

    public void JoystickInput(InputAction.CallbackContext ctx)
    {
        if ((Device == null && Device != ctx.control.device) || _inCoolDown)
        {
            return;
        }
        _inCoolDown = true;

        Vector2 value = ctx.ReadValue<Vector2>();

        if (value == Vector2.zero)
        {
            return;
        }

        if (_currentlySelectedButton == null)
        {
            return;
        }

        var currentSelectable = _currentlySelectedButton.GetComponent<Selectable>();
        float absX = Mathf.Abs(value.x);
        float absY = Mathf.Abs(value.y);

        if (absX < absY && absY > 0.2)
        {
            if (value.y > 0)
            {
                SelectNewButton(currentSelectable.FindSelectableOnUp());
            }
            else
            {
                SelectNewButton(currentSelectable.FindSelectableOnDown());
            }

        }
        else if (absY < absX && absX > 0.2)
        {
            if (value.x > 0)
            {
                SelectNewButton(currentSelectable.FindSelectableOnRight());
            }
            else if (value.x < 0)
            {
                SelectNewButton(currentSelectable.FindSelectableOnLeft());
            }
        }
    }

    private void SelectNewButton(Selectable next)
    {
        if (next != null)
        {
            next.Select();
            _currentlySelectedButton = next.gameObject;
        }
    }
}
