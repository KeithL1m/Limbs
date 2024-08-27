using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Delay(2));
    }

    IEnumerator Delay(int sceneToLoad)
    {
        Debug.Log("Begin Delay");
        yield return new WaitForSeconds(6.2f);

        Debug.Log("End Delay");
        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
