using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    public GameObject descriptionText;

    private int tutorialState;
    private GameObject tutorialEnabledContainer;
    // Start is called before the first frame update
    void Start()
    {
        tutorialState = 0;

        tutorialEnabledContainer = GameObject.Find("TutorialEnabledContainer");

        //Display first set of text here
        descriptionText.GetComponent<TextMeshProUGUI>().text = "Welcome to A Lost World!\nPress the Return Key to Continue  <sprite index=0>";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            switch(tutorialState)
            {
                case 0:
                    descriptionText.GetComponent<TextMeshProUGUI>().text = "A Lost World is a survival game about fighting through a world overrun by aliens and" +
                        " bandits to reach the Golden City, the last bastion of human civilization.  <sprite index=0>";
                    tutorialState++;
                    break;

                case 1:
                    descriptionText.GetComponent<TextMeshProUGUI>().text = "You will first be brought into the World Map, where you can navigate the World by clicking on adjacent hex tiles. When traversing the terrain," +
                        " you will come into contact with enemies of all types, which will take you to the combat screen.  <sprite index=0>";
                    tutorialState++;
                    break;

                case 2:
                    descriptionText.GetComponent<TextMeshProUGUI>().text = "In the Combat Screen, you must Fight to the Death or Run Away from your attacker. Keep in mind that all actions (including healing and trying to run away)" +
                        " consume stamina in different amounts. Typically, stronger attacks consume more stamina. If you run out of stamina, you're a sitting duck, so be sure to manage it wisely! <sprite index=0>";
                    tutorialState++;
                    break;

                case 3:
                    descriptionText.GetComponent<TextMeshProUGUI>().text = "There are two ways to regenerate Stamina. Firstly, you will regenerate Stamina passively, no matter what action you take. At the end of every turn, you will regenerate 10 Stamina." +
                        " You can also choose to Skip your Turn and regenerate an extra 30 Stamina, which will allow you to charge up stronger attacks later in a fight.  <sprite index=0>";
                    tutorialState++;
                    break;

                case 4:
                    descriptionText.GetComponent<TextMeshProUGUI>().text = "After finishing a battle, you automatically fully recover your stamina for the next fight! This is not the case for your health." +
                        " To recover health, you can either move around the map (+10 Health per move) or Heal during battle.  <sprite index=0>";
                    tutorialState++;
                    break;

                case 5:
                    descriptionText.GetComponent<TextMeshProUGUI>().text = "In addition, after every battle you will gain XP. After gaining enough XP, you will level up, which will make you stronger against enemies and grant you" +
                        " new Special powers to fight your enemies.  <sprite index=0>";
                    tutorialState++;
                    break;

                case 6:
                    descriptionText.GetComponent<TextMeshProUGUI>().text = "Lastly, here are the controls for your character.\n\nGeneral:\nQuit Game: Escape\n\nWorld Map:\nMove Space: Left Click\n\nCombat Screen:\nChange Action: Arrow Keys (Alt: Mouse)\nSelect Action: Return (Alt: Left Click)\nGo Back: Backpace  <sprite index=0>";
                    tutorialState++;
                    break;

                case 7:
                    descriptionText.GetComponent<TextMeshProUGUI>().text = "Good Luck and have fun!  <sprite index=0>";
                    tutorialState++;
                    break;

                case 8:
                    tutorialEnabledContainer.GetComponent<TutorialEnabledScript>().tutorialEnabled = false;
                    SceneManager.LoadScene("WorldMap");
                    break;
            }
        }
    }
}
