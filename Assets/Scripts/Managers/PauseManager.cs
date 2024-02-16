using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    GameLoader _loader;
    PauseAction action;

    public static bool paused = false;

    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _arsenalMenu;
    [SerializeField]
    private GameObject _optionsMenu;
    [SerializeField]
    private GameObject _popupMenu;
    [SerializeField]
    private GameObject _exitPopupMenu;

    [SerializeField]
    private EventSystem _eventSystem;

    [SerializeField]
    private GameObject _pauseFirstButton;
    [SerializeField]
    private GameObject _arsenalFirstButton;
    [SerializeField]
    private GameObject _optionsFirstButton;
    [SerializeField]
    private GameObject _popupFirstButton;
    [SerializeField]
    private GameObject _exitPopupFirstButton;


    [SerializeField]
    private Canvas _canvas;

    [SerializeField]
    private UIManager _uiManager;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    void Initialize()
    {
        action = new PauseAction();

        DontDestroyOnLoad(this);
        _pauseMenu.SetActive(false);
        _arsenalMenu.SetActive(false);
        _optionsMenu.SetActive(false);   

        action.Pause.PauseGame.performed += _ => DeterminePause();
    }

    private void OnEnable()
    {
        action?.Enable();
    }

    private void OnDisable()
    {
        action?.Disable();
    }

    private void DeterminePause()
    {
        if (paused) 
        { 
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        Debug.Log("Pausing game");
        Time.timeScale = 0.0f;
        paused = true;
        _pauseMenu.SetActive(true);

        _eventSystem.SetSelectedGameObject(_pauseFirstButton);
    }

    public void ResumeGame()
    {
        Debug.Log("Unpausing game");
        Time.timeScale = 1.0f;
        paused = false;
        _pauseMenu.SetActive(false);
        _arsenalMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _popupMenu.SetActive(false);
        _exitPopupMenu.SetActive(false);
    }

    public void LoadArsenalMenu()
    {
        _pauseMenu.SetActive(false);
        _arsenalMenu.SetActive(true);
        _eventSystem.SetSelectedGameObject(_arsenalFirstButton);
    }

    public void LoadOptionsMenu()
    {
        _pauseMenu.SetActive(false);
        _optionsMenu.SetActive(true);
        _eventSystem.SetSelectedGameObject(_optionsFirstButton);
    }

    public void LoadPauseMenu()
    {
        _arsenalMenu.SetActive(false);
        _optionsMenu.SetActive(false);
        _pauseMenu.SetActive(true);
        _eventSystem.SetSelectedGameObject(_pauseFirstButton);
    }

    public void LoadPopUpMenu()
    {
        _popupMenu.SetActive(true);
        _eventSystem.SetSelectedGameObject(_popupFirstButton);
        //make sure an animation plays when this is clicked
    }

    public void UnloadPopupMenu()
    {
        _popupMenu.SetActive(false);
        _eventSystem.SetSelectedGameObject(_pauseFirstButton);
        //make sure an animation plays when this is clicked
    }

    public void LoadExitPopUpMenu()
    {
        _exitPopupMenu.SetActive(true);
        _eventSystem.SetSelectedGameObject(_exitPopupFirstButton);
        //make sure an animation plays when this is clicked
    }

    public void UnloadExitPopupMenu()
    {
        _exitPopupMenu.SetActive(false);
        _eventSystem.SetSelectedGameObject(_exitPopupFirstButton);
        //make sure an animation plays when this is clicked
    }

    public void VictoryScreen(GameObject button)
    {
        _eventSystem.SetSelectedGameObject(button);
    }

    public void EndGame()
    {
        Debug.Log("Ending Game");
        GameManager gm = ServiceLocator.Get<GameManager>();
        gm.ResetRound();
        gm.EarlyEnd = true;
        gm.EndGame();
        Time.timeScale = 1.0f;
        paused = false;
        Debug.Log("Unpausing game");
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        Debug.Log("Going back to main menu");
        GameManager gm = ServiceLocator.Get<GameManager>();
        gm.ResetRound();
        gm.EarlyEnd = true;
        gm.EndGame();
        Time.timeScale = 1.0f;
        paused = false;
        Debug.Log("Unpausing game");
        SceneManager.LoadScene(1);
    }

    public void SetCamera(Camera camera)
    {
        _canvas.worldCamera = camera;
    }
}
