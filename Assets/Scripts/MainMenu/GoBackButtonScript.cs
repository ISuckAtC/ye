using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBackButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas mainMenu;
    public Canvas settingsMenu;

    public void BackToMainMenu()
    {
        settingsMenu.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }
}
