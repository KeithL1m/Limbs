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

        EventSystem.current.SetSelectedGameObject(_currentlySelectedButton);
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

        Vector2 value = ctx.ReadValue<Vector2>();
        float absX = Mathf.Abs(value.x);
        float absY = Mathf.Abs(value.y);

        if (value == Vector2.zero || absX < 0.5f && absY < 0.5f)
        {
            _inCoolDown = false;
            return;
        }

        if (_currentlySelectedButton == null)
        {
            return;
        }

        
        _inCoolDown = true;
        Debug.Log("Selected new button");
        Debug.Log(value);

        var currentSelectable = _currentlySelectedButton.GetComponent<Button>();

        if (absX < absY && absY > 0.2)
        {
            if (value.y > 0)
            {
                SelectNewButton(currentSelectable.navigation.selectOnUp);
            }
            else
            {
                SelectNewButton(currentSelectable.navigation.selectOnDown);
            }

        }
        else if (absY < absX && absX > 0.2)
        {
            if (value.x > 0)
            {
                SelectNewButton(currentSelectable.navigation.selectOnRight);
            }
            else if (value.x < 0)
            {
                SelectNewButton(currentSelectable.navigation.selectOnLeft);
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
