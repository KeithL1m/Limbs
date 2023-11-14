using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Manager
{
    [SerializeField] private  Player _basePlayer;
    private List<GameObject> _playerObjects = new();

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

    public void AddPlayerObject(GameObject player)
    {
        _playerObjects.Add(player);
    }

    public void AddPlayer(Player player)
    {
        _playerList.Add(player);

        for (int i = 0; i < _playerList.Count; i++)
        {
            _playerList[i].GetComponent<PlayerLimbs>().ClearLimbs();
            _playerList[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            _playerList[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            _playerList[i].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            _playerList[i].GetComponent<PlayerHealth>()._isDead = false;
        }
    }
}


