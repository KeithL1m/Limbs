using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] 
    private GameObject mainMenu;
    [SerializeField] 
    private GameObject arsenalMenu;
    [SerializeField] 
    private GameObject settingsMenu;
    [SerializeField] 
    private GameObject creditsMenu;
    [SerializeField]
    private GameObject gateTransition;
    [SerializeField]
    private GameObject fadeWipeTransition;

    [Header("Menu Selected Buttons")]
    [SerializeField]
    private GameObject selectedButtonMainMenu;
    [SerializeField]
    private GameObject selectedButtonArsenalMenu;
    [SerializeField]
    private GameObject selectedButtonSettingsMenu;
    [SerializeField]
    private GameObject selectedButtonCreditsMenu;

    [SerializeField]
    private string[] buttonDescriptions;

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
        fadeWipeTransition.SetActive(true);
        EventSystem.current.SetSelectedGameObject(selectedButtonMainMenu);
    }

    public void ShowMenu(GameObject panelToShow)
    {
        mainMenu.SetActive(false);
        panelToShow.SetActive(true);
    }

    public void StartGame()
    {
        _configManager.InLoadout = true;

        gateTransition.SetActive(true);
        StartCoroutine(Delay(3));
    }

    public void LoadArsenalMenu()
    {
        ShowMenu(arsenalMenu);
        EventSystem.current.SetSelectedGameObject(selectedButtonArsenalMenu);
    }
    public void LoadSettingsMenu()
    {
        ShowMenu(settingsMenu);
        EventSystem.current.SetSelectedGameObject(selectedButtonSettingsMenu);
    }

    public void LoadCreditsMenu()
    {
        ShowMenu(creditsMenu);
        EventSystem.current.SetSelectedGameObject(selectedButtonCreditsMenu);
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

    IEnumerator Delay(int sceneToLoad)
    {
        Debug.Log("Set gate active");
        // Wait for 3 seconds
        yield return new WaitForSeconds(1f);

        Debug.Log("After Delay");
        SceneManager.LoadScene(sceneToLoad);
    }
}
