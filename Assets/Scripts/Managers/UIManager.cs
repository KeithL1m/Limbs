using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject scoreBox;
    [SerializeField]
    GameObject leaderboard;

    List<GameObject> scoreBoxes;
    List<Vector2> positions;

    [SerializeField]
    float margin;
    float totalMargin;

    public int playerCount;

    private void Start()
    {
        scoreBoxes = new List<GameObject>();
        positions = new List<Vector2>();
    }

    public void SetUpLeaderBoard()
    {
        playerCount = GameManager.instance.GetPlayerCount();

        scoreBoxes.Add(scoreBox);

        for (int i = 1; i < playerCount; i++)
        {
            totalMargin += margin;
            
            var box = Instantiate(scoreBox, Vector3.zero, Quaternion.identity);
            box.transform.SetParent(leaderboard.transform, false);
            box.transform.localPosition = new Vector3(scoreBox.transform.localPosition.x, scoreBox.transform.localPosition.y - totalMargin, scoreBox.transform.localPosition.z);
            scoreBoxes.Add(box);
        }

        for (int i = 0; i < playerCount; i++)
        {
            positions.Add(scoreBoxes[i].transform.position);
        }
    }

    public void UpdateLeaderBoard()
    {
        //get player datas automatically sorts the list by the score
        List<PlayerData> players = GameManager.instance.GetPlayerDatas();

        for (int i = 0; i < playerCount; i++)
        {
            TextMeshProUGUI textMesh = scoreBoxes[i].GetComponentInChildren<TextMeshProUGUI>();

            textMesh.text = players[i].PlayerName + ": " + players[i].Score.ToString("000");
        }
    }
}
