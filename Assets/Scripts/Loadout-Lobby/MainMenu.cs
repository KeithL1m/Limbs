using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject arsenalMenu;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject creditsMenu;


    public GameObject SelectedButton;

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
        EventSystem.current.SetSelectedGameObject(SelectedButton);
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
    }

    public void BackButton()
    {
        mainMenu.SetActive(true);
        arsenalMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(SelectedButton);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
       
    }
}
