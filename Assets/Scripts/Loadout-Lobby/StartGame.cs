using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private GameLoader _loader;
    private ConfigurationManager _configManager;

    public GameObject SelectedButton;

    [SerializeField] private Texture2D _textureToSave;
    [SerializeField] private AudioClip _buttonSound;
    [SerializeField] private AudioClip _titleMusic;
    [SerializeField] private GameObject fadeTransition;
    private string _filePath;

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

        /*if (!Directory.Exists(Application.persistentDataPath + "/DO_NOT_DELETE/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/DO_NOT_DELETE/");

            _filePath = Path.Combine(Application.persistentDataPath + "/DO_NOT_DELETE/", "NEEDED_FILE_DO_NOT_DELETE.png");
            
            byte[] pngData = _textureToSave.EncodeToPNG();

            File.WriteAllBytes(_filePath, pngData);

            Debug.Log("Sprite saved as PNG at: " + _filePath);

            Debug.Log("Directory Created");
        }

        if (!File.Exists(_filePath))
        {
            Application.Quit();
        }*/
    }

    public void LoadGame()
    {
        fadeTransition.SetActive(true);
        ServiceLocator.Get<AudioManager>().PlaySound(_buttonSound, transform.position, SoundType.SFX);
        ServiceLocator.Get<AudioManager>().StartTitleMusic(_titleMusic);
        StartCoroutine(Delay(nextScene));
    }

    IEnumerator Delay(int sceneToLoad)
    {
        Debug.Log("Set transition active");
        // Wait for 3 seconds
        yield return new WaitForSeconds(1.45f);

        Debug.Log("After Delay");
        SceneManager.LoadScene(sceneToLoad);
    }
}
