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
    EventSystem eventSystem;

    [SerializeField]
    GameObject pauseFirstButton;
    [SerializeField]
    GameObject arsenalFirstButton;
    [SerializeField]
    GameObject optionsFirstButton;


    [SerializeField]
    Canvas canvas;

    private void Awake()
    {
        action = new PauseAction();
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
