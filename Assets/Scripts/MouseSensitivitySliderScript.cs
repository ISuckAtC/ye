using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseSensitivitySliderScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void updateSensitivity(float number)
    {
        PlayerPrefs.SetFloat("sensitivity", number);
        PlayerPrefs.Save();
    }
}
