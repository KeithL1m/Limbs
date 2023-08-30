using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Manager
{
    [SerializeField] Player _basePlayer;
    public GameObject arrowIndicatorPrefab;
    List<Player> _playerList = new List<Player>();
    public Player this[int playerNumber] { get { return _playerList[playerNumber]; } }

    public static PlayerManager instance;
    
    public void AddPlayer(Player player)
    {
        _playerList.Add(player);

    }

    void Start()
    {
        // Instantiate arrow indicator and attach it to the player.
        GameObject arrowIndicator = Instantiate(arrowIndicatorPrefab, _basePlayer.transform.position, Quaternion.identity);
        arrowIndicator.transform.SetParent(_basePlayer.transform);
    }

}
