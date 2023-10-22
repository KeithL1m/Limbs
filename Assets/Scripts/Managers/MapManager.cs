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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameManager.instance.OnStart();
    }
}
