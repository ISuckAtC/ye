using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject creditsObjects;

    public void BackToMainMenu()
    {
        creditsObjects.SetActive(!creditsObjects.activeSelf);
    }
}
