using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public Camera[] cameras;
    public float[] cameraDurations;
    public int i = 0;
    public bool isRunning = false;


    void Start()
    {



    }

    // Update is called once per frame    // Update is called once per frame
    void Update()
    {
        if (isRunning == false && i < cameras.Length)
        {
            StartCoroutine(UseCamera(cameras[i], cameraDurations[i], cameras.Length));
        }

    }


    IEnumerator UseCamera(Camera camera, float duration, int maxLenght)
    {
        camera.enabled = true;
        isRunning = true;
        yield return new WaitForSeconds(duration);
        if (i != cameras.Length - 1)
        {
            camera.enabled = false;
            yield return new WaitForSeconds(0.05f);
            cameras[i + 1].enabled = true;
        }
        i++;
        isRunning = false;
    }

}
