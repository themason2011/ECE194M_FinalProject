using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    private GameObject pauseMenu;
    private GameObject gameInfo;
    private GameObject worldMapController;

    private bool isPaused;

    private void Start()
    {
        pauseMenu = GameObject.Find("PauseMenu");
        pauseMenu.SetActive(false);
        gameInfo = GameObject.Find("GameInfo");
        worldMapController = GameObject.Find("WorldMapController");

        isPaused = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!isPaused)
            {
                ActivatePauseMenu();
            }
            else
            {
                DeactivatePauseMenu();
            }
        }
    }

    public void SelectResumeGame()
    {
        DeactivatePauseMenu();
    }

    public void SelectQuitToMenu()
    {

        Destroy(gameInfo.gameObject);
        Destroy(worldMapController.gameObject);
        SceneManager.LoadScene("TitleScreen");
    }

    public void SelectQuitToDesktop()
    {
        Application.Quit();
    }

    public void ActivatePauseMenu()
    {
        if (GameObject.Find("Player") != null)
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().movementDisabled = true;
        }
        AudioListener.pause = true;
        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void DeactivatePauseMenu()
    {
        if (GameObject.Find("Player") != null)
        {
            GameObject.Find("Player").GetComponent<PlayerMovement>().movementDisabled = false;
        }
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
        isPaused = false;
    }
}
