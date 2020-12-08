using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CombatController : MonoBehaviour
{
    public Text playerHealthText;
    public Text playerStaminaText;
    public Text enemyHealthText;

    public GameObject descriptionText;

    public GameObject mainSelection;
    public GameObject attackSelection;
    public GameObject specialSelection;

    public GameObject attackButton;
    public GameObject specialButton;
    public GameObject runAwayButton;
    public GameObject lightAttackButton;
    public GameObject heavyAttackButton;
    public GameObject dragonsBreathButton;
    public GameObject superPunchButton;
    public GameObject healButton;

    private GameInfo gameInfo;
    private bool playerTurn = true;
    private bool combatOver = false;
    private bool playerWon = false;
    private bool combatOverTextDisplayed = false;
    private bool runAway = false;

    void Start()
    {
        gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();

        LoadEvent(gameInfo.scenarioNumber);
    }

    // Update is called once per frame
    void Update()
    {
        //Run combat in here
        //Update Health and Stamina values from playerInfo
        UpdateUI();
        //Go Back if in Attack or Special Selection and player presses Escape
        CheckBackButtonSelect();
        //Checks if user presses enter during description text to advance it and handles what happens
        CheckDescriptionSelect();
        //Check win/lose conditions for combat
        CheckCombatOver();
    }

    private void LoadEvent(int eventNumber)
    {
        //Load CombatInfo based on eventNumber
        //Combat Scenarios will change when more are added, they get added here

        //Load combat variables
        switch (eventNumber)
        {
            case 0:
                gameInfo.combatInfo = new CombatInfo("Bandit", 100, 3, 3, 3, 100);
                break;
            case 1:
                gameInfo.combatInfo = new CombatInfo("Impostor", 200, 5, 5, 5, 150);
                break;
        }
    }

    private void UpdateUI()
    {
        playerHealthText.text = gameInfo.playerInfo.health.ToString();
        playerStaminaText.text = gameInfo.playerInfo.stamina.ToString();
        enemyHealthText.text = gameInfo.combatInfo.enemyHealth.ToString();
    }

    private void CheckBackButtonSelect()
    {
        //If you are in the Attack or Special Selection menu and hit escape, go back to the mainSelection menu
        if(Input.GetKeyDown(KeyCode.Escape) && !descriptionText.activeSelf && !mainSelection.activeSelf)
        {
            //Enable Main Selection
            mainSelection.SetActive(true);

            //Disable either Attack or Special Selection and Select appropriate Selection in Main Selection
            if (attackSelection.activeSelf)
            {
                attackSelection.SetActive(false);
                attackButton.GetComponent<Button>().Select();
            }
            else if(specialSelection.activeSelf)
            {
                specialSelection.SetActive(false);
                specialButton.GetComponent<Button>().Select();
            }
        }
    }

    private void CheckDescriptionSelect()
    {
        //If descriptionText is currently displaying something
        if(descriptionText.activeSelf)
        {
            //If player presses Return
            if(Input.GetKeyDown(KeyCode.Return))
            {
                //If Combat is still going
                if(!combatOver)
                {
                    //If play was just handed over to the player by the enemy or player did an invalid move
                    if(playerTurn)
                    {
                        //If player did not choose run away option or tried and did not succeed in running away
                        if(!runAway)
                        {
                            descriptionText.SetActive(false);
                            mainSelection.SetActive(true);

                            attackButton.GetComponent<Button>().Select();
                        }
                        //Player ran away from combat!
                        else
                        {
                            GameObject.Find("WorldMapController").GetComponent<WorldMapController>().EnableWorldMap();
                            SceneManager.LoadScene("WorldMap");
                        }
                    }
                    //If play was just handed over to the enemy by the player
                    else
                    {
                        descriptionText.SetActive(false);
                        StartEnemyAction();

                        //Regen Stamina for player every round, easier to do here because when you can't heal or run away, the logic for
                        //playerTurn is still called so people could refresh stamina by spamming fail heals or fail run aways
                        RegenStamina();
                    }
                }
                //Player either won or lost 
                else
                {
                    //If player won
                    if (playerWon)
                    {
                        //If the Combat Over Text was Displayed and player hits enter again, go back to World Map
                        if (combatOverTextDisplayed)
                        {
                            GameObject.Find("WorldMapController").GetComponent<WorldMapController>().EnableWorldMap();
                            SceneManager.LoadScene("WorldMap");
                        }
                        //Display Combat Over Text for when Player Wins Combat!
                        else
                        {
                            descriptionText.GetComponent<TextMeshProUGUI>().text = "You Won the Battle!  <sprite index=0>";
                            combatOverTextDisplayed = true;
                        }
                    }
                    //Player lost
                    else
                    {
                        //If the Combat Over Text was Displayed and player hits enter again, quit the application. Game over
                        if (combatOverTextDisplayed)
                        {
                            //TODO: Make this go to title screen instead of quit application
                            Application.Quit();
                        }
                        //Display Combat Over Text for when Player Loses Combat
                        else
                        {
                            descriptionText.GetComponent<TextMeshProUGUI>().text = "You Lost the Battle. Game Over!  <sprite index=0>";
                            combatOverTextDisplayed = true;
                        }
                    }
                }
            }
        }
    }

    private void CheckCombatOver()
    {
        if(gameInfo.playerInfo.health <= 0)
        {
            combatOver = true;
            playerWon = false;
        }
        else if(gameInfo.combatInfo.enemyHealth <= 0)
        {
            combatOver = true;
            playerWon = true;
        }
    }

    public void RegenStamina()
    {
        //Add some stamina regen formula every round, possibly based on Dex?
        int stamina = gameInfo.playerInfo.stamina;
        int maxStamina = gameInfo.playerInfo.maxStamina;

        //Make sure stamina doesn't go over maxStamina
        gameInfo.playerInfo.stamina = stamina + 20 > maxStamina ? maxStamina : stamina + 20;
    }

    public void StartEnemyAction()
    {
        //Add enemy attack AI based on enemy you're fighting in CombatInfo
        gameInfo.playerInfo.health -= 20;

        descriptionText.GetComponent<TextMeshProUGUI>().text = "Enemy dealt " + 20 + " damage!  <sprite index=0>";
        descriptionText.SetActive(true);

        playerTurn = true;
    }

    public void SelectAttack()
    {
        //Do what happens when you select the Attack button
        mainSelection.SetActive(false);
        attackSelection.SetActive(true);

        lightAttackButton.GetComponent<Button>().Select();
    }

    public void SelectLightAttack()
    {
        //Do what happens when you select the Light Attack button
        //Do Light Attack Action
        gameInfo.combatInfo.enemyHealth -= 20;

        attackSelection.SetActive(false);

        descriptionText.GetComponent<TextMeshProUGUI>().text = "You dealt " + 20 + " damage to the enemy!  <sprite index=0>";
        descriptionText.SetActive(true);

        playerTurn = false;
    }

    public void SelectHeavyAttack()
    {
        //Do what happens when you select the Heavy Attack button
        //Do Heavy Attack Action
        gameInfo.combatInfo.enemyHealth -= 40;

        attackSelection.SetActive(false);

        descriptionText.GetComponent<TextMeshProUGUI>().text = "You dealt " + 40 + " damage to the enemy!  <sprite index=0>";
        descriptionText.SetActive(true);

        playerTurn = false;
    }

    public void SelectSpecial()
    {
        //Do what happens when you select the Special button
        //Go to special selection screen
        mainSelection.SetActive(false);
        specialSelection.SetActive(true);

        superPunchButton.GetComponent<Button>().Select();
    }

    public void SelectDragonsBreath()
    {
        //Do what happens when you select the Dragon's Breath button
        //Do Dragon's Breath Action
        gameInfo.combatInfo.enemyHealth -= 60;

        specialSelection.SetActive(false);

        descriptionText.GetComponent<TextMeshProUGUI>().text = "You dealt " + 60 + " damage to the enemy!  <sprite index=0>";
        descriptionText.SetActive(true);

        playerTurn = false;
    }

    public void SelectSuperPunch()
    {
        //Do what happens when you select the Super Punch button
        //Do Super Punch Action
        gameInfo.combatInfo.enemyHealth -= 50;

        specialSelection.SetActive(false);

        descriptionText.GetComponent<TextMeshProUGUI>().text = "You dealt " + 50 + " damage to the enemy!  <sprite index=0>";
        descriptionText.SetActive(true);

        playerTurn = false;
    }

    public void SelectHeal()
    {
        if (gameInfo.playerInfo.stamina < 40)
        {
            mainSelection.SetActive(false);

            descriptionText.GetComponent<TextMeshProUGUI>().text = "Not enough stamina to heal!  <sprite index=0>";
            descriptionText.SetActive(true);

        }
        else
        {
            //Do what happens when you select the Heal button
            gameInfo.playerInfo.stamina -= 40;

            int health = gameInfo.playerInfo.health;
            int maxHealth = gameInfo.playerInfo.maxHealth;
            gameInfo.playerInfo.health = health + 50 > maxHealth ? maxHealth : health + 50;

            mainSelection.SetActive(false);

            descriptionText.GetComponent<TextMeshProUGUI>().text = "You recovered " + 50 + " health!  <sprite index=0>";
            descriptionText.SetActive(true);

            playerTurn = false;
        }
    }

    public void SelectRunAway()
    {
        //Do what happens when you select the Run Away button
        if(gameInfo.playerInfo.stamina < 50)
        {
            mainSelection.SetActive(false);

            descriptionText.GetComponent<TextMeshProUGUI>().text = "Not Enough Stamina to Run Away! Do Something Else.  <sprite index=0>";
            descriptionText.SetActive(true);
        }
        else
        {
            //TODO: Random Chance of escaping BASED ON DEXSTAT
            gameInfo.playerInfo.stamina -= 50;

            float randomEscape = Random.Range(0f, 1f);
            if(randomEscape > 0.5f)
            {
                mainSelection.SetActive(false);

                descriptionText.GetComponent<TextMeshProUGUI>().text = "Got Away Safely!  <sprite index=0>";
                descriptionText.SetActive(true);
                runAway = true;
            }
            else
            {
                mainSelection.SetActive(false);

                descriptionText.GetComponent<TextMeshProUGUI>().text = "Couldn't escape!  <sprite index=0>";
                descriptionText.SetActive(true);
                playerTurn = false;
            }
        }
    }
}
