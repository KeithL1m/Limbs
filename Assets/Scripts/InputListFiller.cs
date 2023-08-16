using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputListFiller : MonoBehaviour
{
    public List<string> filledList = new List<string>();

    private void OnEnable()
    {
        // Subscribe to the button press action.
        //PlayerInputManager.PlayerControls.ButtonAction.started += OnButtonPress;
    }

    private void OnDisable()
    {
        // Unsubscribe from the button press action.
        //PlayerInputManager.PlayerControls.ButtonAction.started -= OnButtonPress;
    }

    private void OnButtonPress(InputAction.CallbackContext context)
    {
        if (context.started) // Check if the button press has started.
        {
            filledList.Add("Button Pressed"); // Add an item to the list.
        }
    }
}
