using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitGameUI : MonoBehaviour
{
    [SerializeField] private Button _yesButton;
    [SerializeField] private Button _noButton;
    // Start is called before the first frame update
    void Start()
    {
        _yesButton.onClick.AddListener(Yes);
        _noButton.onClick.AddListener(No);
    }

    private void Yes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void No()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            GameManager gm = ServiceLocator.Get<GameManager>();
            gm.EarlyEnd = true;
            Time.timeScale = 1.0f;
            AudioManager am = ServiceLocator.Get<AudioManager>();
            am.StopMusic();
            gm.EndGame();
        }
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
