using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [ServerRpc]
    public void ChangeScene(string name)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    [ClientRpc]
    private void NotifyClientsSceneChangeClientRpc(string sceneName)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);

        Debug.Log("Changed2");
        SceneManager.LoadScene(sceneName); // Change the scene on each client
    }
}
