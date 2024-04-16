using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject SelectedButton;

    [SerializeField] private Texture2D _textureToSave;
    private string _filePath;

    public int nextScene;

    private void Awake()
    {
        EventSystem.current.SetSelectedGameObject(SelectedButton);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(nextScene);
    }
}
