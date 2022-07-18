using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string sceneToLoad;
    public float delay;
    public bool startOnAwake;
    public void Awake()
    {
        if (startOnAwake)
        {
            LoadScene();
        }
    }
    IEnumerator LoadSceneE()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadScene()
    {
        StartCoroutine(LoadSceneE());
    }
}
