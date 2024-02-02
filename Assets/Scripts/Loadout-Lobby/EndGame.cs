using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private GameLoader _loader;
    private GameManager _gm;
    public GameObject SelectedButton;
    private PauseManager _pauseManager;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _gm = ServiceLocator.Get<GameManager>();
        _gm.VictoryScreenSelect(SelectedButton);
    }

    public void RestartGame()
    {
        _gm.EndGame();
        SceneManager.LoadScene(1);
    }
}
