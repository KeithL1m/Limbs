using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private List<RectTransform> _scoreBoxes;
    [SerializeField]
    private List<Image> _playerHeads;
    [SerializeField]
    private List<TextMeshProUGUI> _scores;
    public List<Vector2> _positions;
    private List<PlayerConfiguration> _players;

    [SerializeField] private List<GameObject> healthUI = new List<GameObject>();
    [SerializeField] private List<Image> healthImage = new List<Image>();
    [SerializeField] private List<TMP_Text> winsCounter = new List<TMP_Text>();

    [SerializeField] private GameObject gameOverBG;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private string[] gameOverMessages;

    private float originalTimeScale = 1.0f;

    private int playerCount;

    private void Start()
    {
        _positions = new List<Vector2>();
    }

    public void SetPlayerCount(int count)
    {
        playerCount = count;
    }

    public void SetUpLeaderBoard()
    {
        _players = GameManager.instance.GetPlayerConfigs();

        Debug.Log("Setup Leaderboard");

        for (int i = 0; i < playerCount; i++)
        {
            Debug.Log("yo");
            _scoreBoxes[i].gameObject.SetActive(true);
            _positions.Add(_scoreBoxes[i].localPosition);
            _playerHeads[i].sprite = _players[i].Head;
        }
    }

    public void UpdateLeaderBoard()
    {
        Debug.Log("Updating Leaderboard");
        List<PlayerConfiguration> configs = _players;

        configs.Sort((x, y) => y.Score.CompareTo(x.Score));

        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < playerCount; j++)
            {
                if (configs[i] == _players[j])
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

    public void UpdatePlayerWins(List<PlayerConfiguration> pConfigs)
    {
        for (int i = 0; i < playerCount; i++)
        {
            winsCounter[i].text = "Wins: " + pConfigs[i].Score.ToString();
        }
    }

    public IEnumerator ShowGameOverScreen()
    {
        Time.timeScale = 0.5f;
        gameOverBG.SetActive(true);

        // Display a random game over message from the array
        int randomMessageIndex = Random.Range(0, gameOverMessages.Length);
        gameOverText.text = gameOverMessages[randomMessageIndex];

        Color randomColor = new Color(Random.value, Random.value, Random.value);
        gameOverText.color = randomColor;

        // Wait for 5 seconds
        float endTime = Time.realtimeSinceStartup + 5.0f;

        while (Time.realtimeSinceStartup < endTime)
        {
            yield return null;
        }

        Time.timeScale = originalTimeScale;

        gameOverBG.SetActive(false);

        GameManager.instance.EndRound();
    }
}
