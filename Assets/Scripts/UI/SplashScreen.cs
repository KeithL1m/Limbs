using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(Delay(1));
    }

    IEnumerator Delay(int sceneToLoad)
    {
        Debug.Log("Set gate active");
        yield return new WaitForSeconds(6f);

        Debug.Log("After Delay");
        SceneManager.LoadScene(sceneToLoad);
    }
}
