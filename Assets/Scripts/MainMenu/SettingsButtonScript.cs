using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsButtonScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas mainMenu;
    public Canvas settingsMenu;

    public void GoToSettings()
    {
        mainMenu.gameObject.SetActive(false);
        settingsMenu.gameObject.SetActive(true);
    
    }
}
