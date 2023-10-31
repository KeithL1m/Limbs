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
        Player player = _player.GetComponent<Player>();
        player.Initialize(pc);

        SpriteRenderer arrow = player.GetArrow();

        switch (pc.PlayerIndex)
        {
            case 0:
                arrow.color = Color.red;
                break;
            case 1:
                arrow.color = Color.blue;
                break;
            case 2:
                arrow.color = Color.yellow;
                break;
            case 3:
                arrow.color = Color.green;
                break;
            default: break;
        }

        arrow.color = new Color(arrow.color.r, arrow.color.g, arrow.color.b, 0.5f);

        return player;
    }
}
