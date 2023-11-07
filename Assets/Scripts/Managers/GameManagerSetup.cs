using UnityEngine;

public class GameManagerSetup : MonoBehaviour
{
    GameLoader _loader;
    GameManager _gm;

    [SerializeField] private UIManager _uIManager;
    [SerializeField] private  PauseManager _pauseManager;

    private void Start()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _gm  = ServiceLocator.Get<GameManager>();
        _gm.SetUp(_uIManager, _pauseManager);
    }
}
