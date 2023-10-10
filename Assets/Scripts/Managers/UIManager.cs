using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject scoreBox;

    List<GameObject> scoreBoxes;
    List<Vector2> positions;

    [SerializeField]
    float margin;
    float totalMargin;

    int playerCount;

    private void Start()
    {
        playerCount = GameManager.instance.GetPlayerCount();
    }

    public void SetUpLeaderBoard()
    {
        scoreBoxes.Add(scoreBox);

        for (int i = 1; i < playerCount; i++)
        {
            totalMargin += margin;
            scoreBoxes.Add(Instantiate(scoreBox, new Vector3(scoreBox.transform.position.x, scoreBox.transform.position.y - totalMargin, 0), Quaternion.identity));
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
            TextMeshProUGUI textMesh = scoreBoxes[i].GetComponent<TextMeshProUGUI>();

            textMesh.text = players[i].playerName + ": " + players[i].score.ToString("000");
        }
    }
}
