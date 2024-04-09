using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] 
    private GameObject mainMenu;
    [SerializeField] 
    private GameObject arsenalMenu;
    [SerializeField] 
    private GameObject settingsMenu;
    [SerializeField] 
    private GameObject creditsMenu;

    [SerializeField]
    private GameObject selectedButtonMainMenu;
    [SerializeField]
    private GameObject selectedButtonArsenalMenu;

    private GameLoader _loader;
    private ConfigurationManager _configManager;


    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _configManager = ServiceLocator.Get<ConfigurationManager>();
        EventSystem.current.SetSelectedGameObject(selectedButtonMainMenu);
    }

    public void HideMainMenu(GameObject panelToShow)
    {
        mainMenu.SetActive(false);
        panelToShow.SetActive(true);
    }

    public void StartGame(int sceneToLoad)
    {
        _configManager.InLoadout = true;
        SceneManager.LoadScene(sceneToLoad);
    }

    public void ArsenalMenu()
    {
        HideMainMenu(arsenalMenu);
        EventSystem.current.SetSelectedGameObject(selectedButtonArsenalMenu);
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        arsenalMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(selectedButtonMainMenu);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
       
    }
}
