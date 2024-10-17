using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerOnlineHelper : NetworkBehaviour
{
    [SerializeField] private Player _player;

    private NetworkVariable<bool> _flip = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        _flip.OnValueChanged += OnFlipBody;
    }

    public void HelpFlipBody(bool value)
    {
        if (!IsOwner)
        {
            return;
        }

        _flip.Value = value;
    }


    private void OnFlipBody(bool oldValue, bool newValue)
    {
        _player.FlipSprite(newValue);
    }

    [ServerRpc(RequireOwnership = true)]
    public void HelpFlipBodyServerRpc(bool value)
    {
        _flip.Value = value;
    }
}
