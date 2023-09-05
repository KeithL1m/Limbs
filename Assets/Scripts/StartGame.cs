using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public int nextScene;
    public void LoadGame()
    {
        SceneManager.LoadScene(nextScene);
    }
}
