using UnityEngine.InputSystem;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    float _pauseInput;

    [SerializeField]
    GameObject pauseMenu;

    private void Start()
    {
        DontDestroyOnLoad(this); 
        DontDestroyOnLoad(pauseMenu);
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (_pauseInput >= 0.5f)
        {
            pauseMenu.SetActive(true);
        }
    }

    public void PauseInput(InputAction.CallbackContext ctx) => _pauseInput = ctx.action.ReadValue<float>();
}
