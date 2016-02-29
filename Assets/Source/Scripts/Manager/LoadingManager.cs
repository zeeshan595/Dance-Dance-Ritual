using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public static string sceneToLoad = "Main";

    public IEnumerator Start()
    {
        yield return SceneManager.LoadSceneAsync(sceneToLoad);
        Debug.Log("Scene Loaded");
    }

    public static void ChangeScene(string scene)
    {
        sceneToLoad = scene;
        SceneManager.LoadScene("Loading");
    }
}