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

    [Header("Sounds")]
    [SerializeField] private AudioClip _backButtonSound;
    [SerializeField] private AudioClip _enterButtonSound;
    [SerializeField] private AudioClip _meatcaseMusic;
    private AudioManager _audioManager;



    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _configManager = ServiceLocator.Get<ConfigurationManager>();
        _audioManager = ServiceLocator.Get<AudioManager>();
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
        //change music
        ServiceLocator.Get<AudioManager>().MeatcaseMusic(_meatcaseMusic);
        _configManager.InLoadout = true;

        gateTransition.SetActive(true);
        StartCoroutine(Delay(3));
    }

    public void LoadArsenalMenu()
    {
        _audioManager.PlaySound(_enterButtonSound, transform.position, SoundType.SFX);
        ShowMenu(arsenalMenu);
        EventSystem.current.SetSelectedGameObject(selectedButtonArsenalMenu);
    }
    public void LoadSettingsMenu()
    {
        _audioManager.PlaySound(_enterButtonSound, transform.position, SoundType.SFX);
        ShowMenu(settingsMenu);
        EventSystem.current.SetSelectedGameObject(selectedButtonSettingsMenu);
    }

    public void LoadCreditsMenu()
    {
        _audioManager.PlaySound(_enterButtonSound, transform.position, SoundType.SFX);
        ShowMenu(creditsMenu);
        EventSystem.current.SetSelectedGameObject(selectedButtonCreditsMenu);
    }

    public void BackButton()
    {
        _audioManager.PlaySound(_backButtonSound, transform.position, SoundType.SFX, 0.5f);
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
