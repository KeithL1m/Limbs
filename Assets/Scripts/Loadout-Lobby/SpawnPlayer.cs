using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private PlayerInput _input;

    public GameObject Player => _player;
    private GameObject _player;

    public Player SpawnPlayerFirst(PlayerConfiguration pc)
    {
        _player = Instantiate(_playerPrefab);
        _player.name = $"Player {pc.PlayerIndex}";
        Player player = _player.GetComponent<Player>();
        player.Initialize(pc);

        SpriteRenderer arrow = player.GetArrow();

        Debug.Log(pc.PlayerIndex);

        switch (pc.PlayerIndex)
        {
            case 0:
                arrow.color = new Color(1f, 0.3f, 0.3f, 0.5f);
                break;
            case 1:
                arrow.color = new Color(0.3f, 0.8f, 1f, 0.5f);
                break;
            case 2:
                arrow.color = new Color(1f, 1f, 0.3f, 0.5f);
                break;
            case 3:
                arrow.color = new Color(0.4f, 1f, 0.3f, 0.5f);
                break;
            default: break;
        }

        return player;
    }
}
