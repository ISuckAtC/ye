using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public string[] lines;
    public string[] persons;

    public float[] lineDurations;
    public int i = 0;
    public bool isLineOnScreen = false;

    public TMPro.TMP_Text dialogArea;
    public TMPro.TMP_Text personTalkingArea;


    void Start()
    {


    }

    // Update is called once per frame    // Update is called once per frame
    void Update()
    {
        if (isLineOnScreen == false && i < lines.Length)
        {
            StartCoroutine(ShowLine(lines[i], lineDurations[i]));
        }

    }


    IEnumerator ShowLine(string line, float duration)
    {
        isLineOnScreen = true;
        dialogArea.text = lines[i];
        personTalkingArea.text = persons[i];
        yield return new WaitForSeconds(duration);
        i++;
        isLineOnScreen = false;
    }

}
