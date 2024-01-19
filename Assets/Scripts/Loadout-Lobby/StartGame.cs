using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private GameLoader _loader;
    private ConfigurationManager _configManager;

    public GameObject SelectedButton;

    public int nextScene;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _configManager = ServiceLocator.Get<ConfigurationManager>();
        EventSystem.current.SetSelectedGameObject(SelectedButton);
    }

    public void LoadGame()
    {
        _configManager.InLoadout = true;
        SceneManager.LoadScene(nextScene);
    }
}
