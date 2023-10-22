using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Manager
{
    [SerializeField] Player _basePlayer;
    public GameObject arrowIndicatorPrefab;
    

    public List<Player> _playerList = new List<Player>();

    public static PlayerManager instance;
    
    public void AddPlayer(Player player)
    {
        _playerList.Add(player);

        for(int i = 0; i < _playerList.Count; i++)
        {
            _playerList[i].GetComponent<PlayerLimbs>().ClearLimbs();
            _playerList[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            _playerList[i].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            _playerList[i].GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
            _playerList[i].GetComponent<PlayerHealth>()._isDead = false;
            
        }


    }

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        // Instantiate arrow indicator and attach it to the player.
        //GameObject arrowIndicator = Instantiate(arrowIndicatorPrefab, _basePlayer.transform.position, Quaternion.identity);
        //arrowIndicator.transform.SetParent(_basePlayer.transform);
        
    }

}


