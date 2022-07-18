using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI, optionsMenuUI;
    public Slider mouseSensitivitySlider;
    public bool isInOptionsMenu = false;
    private PlayerController pC;

    void Start()
    {
        pC = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused && isInOptionsMenu == true)
            {
                Resume();
            }
            else if(!gameIsPaused && isInOptionsMenu == false)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;

        pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;

        gameIsPaused = false;

        isInOptionsMenu = false;
    }

    void Pause()
    {
        isInOptionsMenu = true;

        Cursor.lockState = CursorLockMode.Confined;

        pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;

        gameIsPaused = true;
    }

    public void Options()
    {
        isInOptionsMenu = false;

        pauseMenuUI.SetActive(false);

        optionsMenuUI.SetActive(true);
    }

    public void GoBack()
    {
        isInOptionsMenu = true;

        pauseMenuUI.SetActive(true);

        optionsMenuUI.SetActive(false);      
    }

    public void Slider()
    {
        pC.mouseSensitivity = mouseSensitivitySlider.value;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}
