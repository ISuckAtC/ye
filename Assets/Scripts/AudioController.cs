using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public List<AudioClip> clipsLibrary;

    public AudioClip[] clips;

    public AudioClip initialClip;
    public AudioSource audioSource;

    public string state = "introducing";

    public int i = 0;


    void Start()
    {
        audioSource.clip = initialClip;
        audioSource.Play();
        StartCoroutine(AwaitForPlay(audioSource.clip.length));

    }

    void Update()
    {
        if (state == "introducing")
            return;

        if (state == "awaitingNewClip")
        {
            audioSource.clip = clips[i];
            state = "looping";
            audioSource.Play();
            StartCoroutine(AwaitForPlay(audioSource.clip.length));

            i++;
            if (i == clips.Length)
                i = 0;
        }

    }

    //on this clipindices you pass indices from the clipsLibrary;
    //TODO ALLOW A INTRO CLIP FOR NEW LOOOP AND OUTRO CLIP FOR PREVIOUS LOOOOP
    public void defineNewLoops(int[] clipIndices)
    {
        clips = new AudioClip[clipIndices.Length];
        var iteratorForMe = 0;
        foreach (var index in clipIndices)
        {
            clips[iteratorForMe] = clipsLibrary[index];
            iteratorForMe++;
        }
        i = 0;
    }

    IEnumerator AwaitForPlay(float duration)
    {
        Debug.Log("coroutine starter");
        yield return new WaitForSeconds(duration);
        state = "awaitingNewClip";
    }

}
