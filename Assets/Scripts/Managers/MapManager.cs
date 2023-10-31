using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : Manager
{
    public int _mapCount;

    private static System.Random rnd = new System.Random();

    public static MapManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadMap()
    {
        int mapNum = rnd.Next(3, _mapCount + 1);
        SceneManager.LoadScene(mapNum);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameManager.instance.startScreen)
            return;
        Debug.Log("new scene loaded");
        GameManager.instance.OnStart();
    }
}
