using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Manager
{
    [SerializeField] Player _basePlayer;
    List<Player> _playerList = new List<Player>();
    public Player this[int playerNumber] { get { return _playerList[playerNumber]; } }
    
    public void AddPlayer(Player player)
    {
        _playerList.Add(player);
    }

    void Update()
    {
        if(true) // player input condition
        {
            AddPlayer(_basePlayer);
        }
    }
}
