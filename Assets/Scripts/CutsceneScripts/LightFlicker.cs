using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light isMyLight;
    private float[] smoothing = new float[20];

    void Start()
    {
        // Initialize the array.
        for (int i = 0; i < smoothing.Length; i++)
        {
            smoothing[i] = .0f;
        }
    }

    void Update()
    {
        float sum = .0f;

        for (int i = 1; i < smoothing.Length; i++)
        {
            smoothing[i - 1] = smoothing[i];
            sum += smoothing[i - 1];
        }

        smoothing[smoothing.Length - 1] = Random.value;
        sum += smoothing[smoothing.Length - 1];

        isMyLight.intensity = sum / smoothing.Length;
    }
}
