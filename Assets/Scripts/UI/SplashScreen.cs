using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private void Awake()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int nextScene = currentScene.buildIndex + 1;
        StartCoroutine(Delay(nextScene));
    }

    IEnumerator Delay(int sceneToLoad)
    {
        Debug.Log("Begin Delay");
        yield return new WaitForSeconds(6.2f);

        Debug.Log("End Delay");
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
