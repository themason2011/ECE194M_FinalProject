using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScript : MonoBehaviour
{
    public Toggle disableTutorial;

    private GameObject tutorialEnabledContainer;

    private void Awake()
    {
        AudioListener.pause = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        tutorialEnabledContainer = GameObject.Find("TutorialEnabledContainer");

        disableTutorial.isOn = !tutorialEnabledContainer.GetComponent<TutorialEnabledScript>().tutorialEnabled;
    }

    public void NewGame()
    {
        if(tutorialEnabledContainer.GetComponent<TutorialEnabledScript>().tutorialEnabled)
        {
            SceneManager.LoadScene("TutorialScene");
        }
        else
        {
            SceneManager.LoadScene("WorldMap");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
