using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    public Text healthText;
    public Text staminaText;

    private GameObject gameController;
    private GameController gameControllerComponent;
    private GameObject playerController;

    //0 to 3, 0 -> Action, 1 -> Item, 2 -> Special, 3 -> Run Away
    private int actionSelect;

    void Start()
    {
        gameController = GameObject.Find("GameController");
        gameControllerComponent = gameController.GetComponent<GameController>();
        playerController = GameObject.Find("PlayerController");

        actionSelect = 0;

        LoadEvent(gameController.GetComponent<GameController>().scenarioNumber);
    }

    private void LoadEvent(int eventNumber)
    {
        //Load CombatInfo based on eventNumber
        //Combat Scenarios will change when more are added, they get added here.
        GameController gameControllerComponent = gameController.GetComponent<GameController>();

        //Load combat variables
        switch (eventNumber)
        {
            case 0:
                gameControllerComponent.combatInfo = new CombatInfo("Bandit", 10, 3, 3, 3, 100);
                break;
            case 1:
                gameControllerComponent.combatInfo = new CombatInfo("Impostor", 20, 5, 5, 5, 150);
                break;
        }
    }

    private void UpdateUI()
    {
        healthText.text = gameControllerComponent.playerInfo.health.ToString();
        staminaText.text = gameControllerComponent.playerInfo.stamina.ToString();
    }

    private void SelectAction()
    {

    } 

    // Update is called once per frame
    void Update()
    {
        //Run combat in here
        UpdateUI();
        SelectAction();
    }
}
