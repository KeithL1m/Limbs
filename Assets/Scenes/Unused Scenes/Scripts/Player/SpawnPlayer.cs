using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private PlayerInput _input;
    private GameObject _player;

    public Player SpawnPlayerFirst(PlayerConfiguration pc)
    {
        _player = Instantiate(_playerPrefab);
        return _player.GetComponent<Player>().Initialize(pc);
    }
}
