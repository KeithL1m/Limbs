using UnityEngine;
using UnityEngine.InputSystem;

public class AIButton : MonoBehaviour
{
    private ConfigurationManager _configManager;
    [SerializeField] private GameObject _playerConfig;
    GameObject ai = null;

    private void Awake()
    {
        GameLoader loader = ServiceLocator.Get<GameLoader>();
        loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _configManager = ServiceLocator.Get<ConfigurationManager>();    
    }

    public void AddAI()
    {
        if (_configManager.GetPlayerNum() > 4)
        {
            return;
        }
        ai =  Instantiate(_playerConfig);
        ai.GetComponent<PlayerInput>().enabled = false;
    }
}
