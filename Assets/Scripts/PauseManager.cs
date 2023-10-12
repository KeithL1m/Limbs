using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    PauseAction action;

    public static bool paused = false;

    [SerializeField]
    GameObject pauseMenu;
    [SerializeField]
    GameObject arsenalMenu;
    [SerializeField]
    GameObject optionsMenu;
    [SerializeField]
    GameObject popupMenu;

    [SerializeField]
    EventSystem eventSystem;

    [SerializeField]
    GameObject pauseFirstButton;
    [SerializeField]
    GameObject arsenalFirstButton;
    [SerializeField]
    GameObject optionsFirstButton;
    [SerializeField]
    GameObject popupFirstButton;


    [SerializeField]
    Canvas canvas;

    private UIManager _uiManager;

    private void Awake()
    {
        action = new PauseAction();

        _uiManager = FindObjectOfType<UIManager>();
    }

    void Start()
    {   
        DontDestroyOnLoad(this);
        pauseMenu.SetActive(false);
        arsenalMenu.SetActive(false);
        optionsMenu.SetActive(false);   

        action.Pause.PauseGame.performed += _ => DeterminePause();
    }

    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
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
        Time.timeScale = 0.0f;
        paused = true;
        pauseMenu.SetActive(true);
        _uiManager.SetUpLeaderBoard();
        _uiManager.UpdateLeaderBoard();
        eventSystem.SetSelectedGameObject(pauseFirstButton);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        paused = false;
        pauseMenu.SetActive(false);
        arsenalMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void LoadArsenalMenu()
    {
        pauseMenu.SetActive(false);
        arsenalMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(arsenalFirstButton);
    }

    public void LoadOptionsMenu()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(optionsFirstButton);
    }

    public void LoadPauseMenu()
    {
        arsenalMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(pauseFirstButton);
    }

    public void LoadPopUpMenu()
    {
        popupMenu.SetActive(true);
        eventSystem.SetSelectedGameObject(popupFirstButton);
        //make sure an animation plays when this is clicked
    }

    public void UnloadPopupMenu()
    {
        popupMenu.SetActive(false);
        eventSystem.SetSelectedGameObject(pauseMenu);
        //make sure an animation plays when this is clicked
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(1);
        ResumeGame();
    }

    public void SetCamera(Camera camera)
    {
        canvas.worldCamera = camera;
    }
}
