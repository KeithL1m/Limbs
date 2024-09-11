using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//Temporary name
public abstract class ConfigurationManagerBase : MonoBehaviour
{
    public abstract bool InLoadout { get; set; }
    //Change this to just configuration manager
    public abstract ConfigurationManagerBase Initialize();

    public abstract void SetPlayerHead(int index, Sprite head);

    public abstract void SetPlayerBody(int index, Sprite body);

    public abstract void SetPlayerName(int index, string name);

    public abstract void ReadyPlayer(int index);

    public abstract bool HandlePlayerJoin(InputDevice device, GameObject prefab);

    public abstract void JoinPlayer(GameObject player, InputDevice device, int playerNum);

    public abstract void ResetConfigs();

    public abstract List<PlayerConfiguration> GetPlayerConfigs();

    public abstract int GetPlayerNum();
}