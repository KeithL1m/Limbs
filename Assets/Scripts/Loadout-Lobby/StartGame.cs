using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private GameLoader _loader;
    private ConfigurationManager _configManager;

    public GameObject SelectedButton;

    [SerializeField] private Texture2D _textureToSave;
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
        SceneManager.LoadScene(nextScene);
    }
}
