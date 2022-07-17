using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake( ){
        PlayerPrefs.SetFloat("sensitivity", 1f);
    }
}
