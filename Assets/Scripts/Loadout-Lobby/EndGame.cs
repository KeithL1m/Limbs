using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private GameLoader _loader;
    private GameManager _gm;
    public GameObject SelectedButton;

    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        _gm = ServiceLocator.Get<GameManager>();
        EventSystem.current.SetSelectedGameObject(SelectedButton);
    }

    public void RestartGame()
    {
        _gm.EndGame();
        SceneManager.LoadScene(1);
    }
}
