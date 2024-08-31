using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Manager
{
    private List<GameObject> _playerObjects = new();
    private List<GameObject> _spawnPoints = new();

    public List<Player> _playerList = new ();
    public static PlayerManager instance;

    public PlayerManager Initialize()
    {
        return this;
    }

    public List<GameObject> GetPlayerObjects()
    {
        return _playerObjects;
    }

    public void SetSpawnPoints(List<GameObject> spawns)
    {
        _spawnPoints = new List<GameObject>(spawns);
    }

    public void AddPlayerObject(GameObject player)
    {
        _playerObjects.Add(player);
        _playerList.Add(player.GetComponent<Player>());
    }

    public void OnStartRound()
    {
        for (int i = 0; i < _playerList.Count; i++)
        {
            Debug.Log($"<color=lime>Player {i} is being spawned</color>");
            _playerList[i].GetComponent<PlayerHealth>().ResetHealth();
            SpawnPlayer(i);
        }
    }

    public void ClearLimbs()
    {
        for (int i = 0; i < _playerList.Count; i++)
        {
            _playerList[i].GetComponent<PlayerLimbs>().ClearLimbs();
        }
    }

    public void ResetGroundCheck()
    {
        for (int i = 0; i < _playerList.Count; i++)
        {
            _playerList[i].GroundCheckTransform.localPosition = new Vector3(0, -0.715f, 0);
        }
    }

    private void SpawnPlayer(int playerNum)
    {
        _playerList[playerNum].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        _playerList[playerNum].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        _playerList[playerNum].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
        _playerList[playerNum].GetComponent<PlayerHealth>().isDead = false;
        _playerList[playerNum].transform.position = _spawnPoints[playerNum].transform.position;
       
        if (!ServiceLocator.Get<GameManager>().VictoryScreen)
        {
            return;
        }

        if (playerNum == 0)
        {
            _playerList[playerNum].SetDisplayCrown(true);
        }
        else
        {
            _playerList[playerNum].SetDisplayCrown(false);
        }
    }

    public bool PlayerHasWon(int wins)
    {
        for (int j = 0; j < _playerList.Count; j++)
        {
            if (_playerList[j].GetScore() == wins)
            {
                return true;
            }
        }

        return false;
    }

    public void SortPlayers()
    {
        _playerList.Sort((emp2, emp1) => emp1.GetScore().CompareTo(emp2.GetScore()));
    }

    public void ClearLists()
    {
        _playerList.Clear();
        _playerObjects.Clear();
        _spawnPoints.Clear();
    }
}


