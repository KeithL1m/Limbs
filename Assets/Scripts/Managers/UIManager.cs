using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _scoreBox;
    [SerializeField]
    private GameObject _leaderboard;

    private List<GameObject> _scoreBoxes;
    private List<Vector2> _positions;

    [SerializeField]
    private float _margin;
    private float _totalMargin;

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
        _scoreBoxes = new List<GameObject>();
        _positions = new List<Vector2>();
    }

    public void SetUpLeaderBoard()
    {
        playerCount = GameManager.instance.GetPlayerCount();

        _scoreBoxes.Add(_scoreBox);

        for (int i = 1; i < playerCount; i++)
        {
            _totalMargin += _margin;
            
            var box = Instantiate(_scoreBox, Vector3.zero, Quaternion.identity);
            box.transform.SetParent(_leaderboard.transform, false);
            box.transform.localPosition = new Vector3(_scoreBox.transform.localPosition.x, _scoreBox.transform.localPosition.y - _totalMargin, _scoreBox.transform.localPosition.z);
            _scoreBoxes.Add(_scoreBox);
        }

        for (int i = 0; i < playerCount; i++)
        {
            _positions.Add(_scoreBoxes[i].transform.position);
        }
    }

    public void UpdateLeaderBoard()
    {
        //get player datas automatically sorts the list by the score
        List<PlayerConfiguration> players = GameManager.instance.GetPlayerConfigs();

        for (int i = 0; i < playerCount; i++)
        {
            TextMeshProUGUI textMesh = _scoreBoxes[i].GetComponentInChildren<TextMeshProUGUI>();

            textMesh.text = players[i].Name + ": " + players[i].Score.ToString("000");
        }
    }

    public void SetUpHealthUI(List<Player> players)
    {
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
