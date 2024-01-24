using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private GameLoader _loader = null;
    private GameManager _gm = null;

    [SerializeField] private List<RectTransform> _scoreBoxes;
    [SerializeField] private List<Image> _playerHeads;
    [SerializeField] private List<TextMeshProUGUI> _scores;

    private List<Vector2> _positions = new List<Vector2>();
    private List<PlayerConfiguration> _players;

    [SerializeField] private List<GameObject> healthUI = new List<GameObject>();
    [SerializeField] private List<Image> healthImage = new List<Image>();
    [SerializeField] private List<TMP_Text> winsCounter = new List<TMP_Text>();

    [SerializeField] private GameObject gameOverBG;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private string[] gameOverMessages;

    [SerializeField] private SceneFade _fade;

    private float originalTimeScale = 1.0f;

    private int playerCount;


    private void Awake()
    {
        _loader = ServiceLocator.Get<GameLoader>();
        _loader.CallOnComplete(Initialize);
    }

    private void Initialize()
    {
        Debug.Log($"{nameof(Initialize)}");

        _gm = ServiceLocator.Get<GameManager>();
    }

    public void SetPlayerCount(int count)
    {
        playerCount = count;
    }

    public void SetUpLeaderBoard()
    {
        _players = _gm.GetPlayerConfigs();

        Debug.Log("Setup Leaderboard");

        for (int i = 0; i < playerCount; i++)
        {
            _scoreBoxes[i].gameObject.SetActive(true);
            _positions.Add(_scoreBoxes[i].localPosition);
            _playerHeads[i].sprite = _players[i].Head;
        }
    }

    public void UpdateLeaderBoard()
    {
        Debug.Log("Updating Leaderboard");
        List<KeyValuePair<string, int>> scores = new List<KeyValuePair<string, int>>();
        for (int i = 0; i < playerCount; i++)
        {
            scores.Add(new KeyValuePair<string, int>(_players[i].Name, _players[i].Score));
        }

        scores.Sort((x, y) => y.Value.CompareTo(x.Value));

        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < playerCount; j++)
            {
                if (scores[i].Key == _players[j].Name)
                {
                    _scoreBoxes[j].localPosition = _positions[j];
                    break;
                }
            }
            _scores[i].text = "Wins: " + _players[i].Score;
        }
    }

    public void SetUpHealthUI(List<Player> players)
    {
        Debug.Log(playerCount);

        for (int i = 0; i < playerCount; i++)
        {
            healthUI[i].SetActive(true);// Enable the health UI for the newly joined player

            // Retrieve the Slider component from the instantiated health UI
            Slider healthSlider = healthUI[i].GetComponentInChildren<Slider>();

            // Here, you would set the health value on the player.
            players[i].GetComponent<PlayerHealth>().SetHealthSlider(healthSlider);

            // Update the health value on the slider
            PlayerHealth playerHealth = players[i].GetComponent<PlayerHealth>();
            float initialHealth = playerHealth._maxHealth;
            healthSlider.value = initialHealth;
        }
    }

    public void SetPlayerHealthFace(List<PlayerConfiguration> pConfigs)
    {
        for (int i = 0; i < playerCount; i++)
        {
            healthImage[i].sprite = pConfigs[i].Head;
        }
    }

    public void UpdatePlayerWins()
    {
        for (int i = 0; i < playerCount; i++)
        {
            winsCounter[i].text = "Wins: " + _players[i].Score.ToString();
        }
    }

    public IEnumerator ShowGameOverScreen(PlayerConfiguration winningConfig)
    {
        Time.timeScale = 0.5f;
        gameOverBG.SetActive(true);

        // Display a random game over message from the array
        int randomMessageIndex = Random.Range(0, gameOverMessages.Length);
        gameOverText.text = gameOverMessages[randomMessageIndex];

        //set colour to winning player colour
        Color color = Color.white;
        switch(winningConfig.PlayerIndex)
        {
            case 0:
                color = new Color(1f, 0.3f, 0.3f, 0.5f);
                break;
            case 1:
                color = new Color(0.3f, 0.8f, 1f, 0.5f);
                break;
            case 2:
                color = new Color(1f, 1f, 0.3f, 0.5f);
                break;
            case 3:
                color = new Color(0.4f, 1f, 0.3f, 0.5f);
                break;
            default: break;
        }
        gameOverText.color = color;

        // Wait for 5 seconds
        float endTime = Time.realtimeSinceStartup + 5.0f;

        while (Time.realtimeSinceStartup < endTime)
        {
            yield return null;
        }

        Time.timeScale = originalTimeScale;

        gameOverBG.SetActive(false);

        _gm.EndRound();
    }

    public void ResetPlayerUI()
    {
        for (int i = 0; i < playerCount; i++)
        {
            winsCounter[i].text = "Wins: 0";
            healthUI[i].SetActive(false);
            _scores[i].text = "Wins: 0";
            _scoreBoxes[i].gameObject.SetActive(false);
        }

        _players.Clear();
        _positions.Clear();
    }

    public SceneFade GetFade()
    {
        return _fade;
    }
}
