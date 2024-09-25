using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    [Header("Input Handler")]
    [SerializeField] private InputActionAsset _inputActionAsset;
    private InputActionMap _actionMap;
    private InputAction _inputAction;

    public delegate void InputEventHandler();
    private InputEventHandler _joinEvents;

    [Space, Header("Helpers")]
    [SerializeField] private ConfigurationManager _configurationManager;
    [SerializeField] private ConfigurationManagerOnline _configurationManagerOnline;

    [SerializeField] private GameObject _gameObjectToInitWithNewInput;

    [SerializeField] private GameManager _gameManager;

    private void Awake()
    {
        GameLoader gm = ServiceLocator.Get<GameLoader>();
        gm.CallOnComplete(Initialize);
    }

    void Initialize()
    {
        _actionMap = _inputActionAsset.FindActionMap("Gameplay");
        _inputAction = _actionMap.FindAction("AnyInput");
        _inputAction.performed += OnInitPreformed;
        _inputAction.Enable();
    }

    private void OnDestroy()
    {
        _inputAction.Disable();
        _inputAction.performed -= OnInitPreformed;
    }

    private void OnInitPreformed(InputAction.CallbackContext context)
    {
        InputDevice device = context.control.device;

        if (_gameManager.IsOnline)
        {
            _configurationManagerOnline.HandlePlayerJoin(device);
        }
        else
        {
            _configurationManager.HandlePlayerJoin(device, _gameObjectToInitWithNewInput);
        }
    }
}
