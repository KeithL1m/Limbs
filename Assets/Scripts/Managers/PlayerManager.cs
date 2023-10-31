using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Manager
{
    [SerializeField] Player _basePlayer;
    public GameObject arrowIndicatorPrefab;
    

    public List<PlayerInput> _playerInputList = new List<PlayerInput>();

    public static PlayerManager instance;
    
    public void AddPlayer(PlayerInput player)
    {
        _playerInputList.Add(player);

        for(int i = 0; i < _playerInputList.Count; i++)
        {
            _playerInputList[i].GetComponent<PlayerLimbs>().ClearLimbs();
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
        
    }

}


