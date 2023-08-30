using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Manager
{
    [SerializeField] Player _basePlayer;
    public GameObject arrowIndicatorPrefab;
    List<Player> _playerList = new List<Player>();
    public List<PlayerInput> _playerInputList = new List<PlayerInput>();
    public Player this[int playerNumber] { get { return _playerList[playerNumber]; } }

    public static PlayerManager instance;
    
    public void AddPlayer(PlayerInput player)
    {
        _playerInputList.Add(player);

        for(int i = 0; i < _playerInputList.Count; i++)
        {
            _playerInputList[i].GetComponent<Player>().ClearLimbs();
            _playerInputList[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            _playerInputList[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            _playerInputList[i].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            _playerInputList[i].GetComponent<PlayerHealth>()._isDead = false;
        }


    }

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        // Instantiate arrow indicator and attach it to the player.
        GameObject arrowIndicator = Instantiate(arrowIndicatorPrefab, _basePlayer.transform.position, Quaternion.identity);
        arrowIndicator.transform.SetParent(_basePlayer.transform);
    }

}
