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
    public GameObject healAndRunSelection;

    public GameObject attackButton;
    public GameObject specialButton;
    public GameObject healAndRunButton;
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
    private bool actionNotPossible = false;
    private bool runAway = false;
    private bool skipTurn = false;

    void Awake()
    {
        gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();

        //Reset stamina to max at start of match
        gameInfo.playerInfo.stamina = gameInfo.playerInfo.maxStamina;

        LoadEvent(gameInfo.scenarioNumber);
    }

    // Update is called once per frame
    void Update()
    {
        //Run combat in here
        //Update Health and Stamina values from playerInfo
        UpdateUI();
        //Go Back if in Attack or Special Selection and player presses Backspace
        CheckBackButtonSelect();
        //Checks if combat has been won or lost
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
        //If you are in the Attack or Special Selection menu and hit backspace, go back to the mainSelection menu
        if(Input.GetKeyDown(KeyCode.Backspace) && !descriptionText.activeSelf && !mainSelection.activeSelf)
        {
            //Enable Main Selection
            mainSelection.SetActive(true);

            //Disable Attack, Special, or Heal/Run Selection and Select appropriate Selection in Main Selection
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
            else if(healAndRunSelection.activeSelf)
            {
                healAndRunSelection.SetActive(false);
                healAndRunButton.GetComponent<Button>().Select();
            }
        }
    }

    public void SelectDescriptionText()
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

                            if(!actionNotPossible)
                            {
                                //Regen Stamina if this is the start of player's turn
                                if (skipTurn)
                                {
                                    skipTurn = false;
                                    //TODO: Decide if I want to remove passive stamina regen when you skip a turn to regen stamina
                                    RegenStamina(10);
                                }
                                else
                                {
                                    RegenStamina(10);
                                }
                            }
                            else
                            {
                                //Don't regen stamina, just reset flag for healing or running away failure
                                actionNotPossible = false;
                            }
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
                            //Destroy info for this game because player lost and return to title screen
                            Destroy(gameInfo.gameObject);
                            GameObject worldMapController = GameObject.Find("WorldMapController");
                            Destroy(worldMapController);
                            SceneManager.LoadScene("TitleScreen");
                        }
                        //Display Combat Over Text for when Player Loses Combat
                        else
                        {
                            descriptionText.GetComponent<TextMeshProUGUI>().text = "You Died. Game Over!  <sprite index=0>";
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

    private void RegenStamina(int staminaRegen)
    {
        //Add some stamina regen formula every round, possibly based on Dex?
        int stamina = gameInfo.playerInfo.stamina;
        int maxStamina = gameInfo.playerInfo.maxStamina;

        //Make sure stamina doesn't go over maxStamina
        gameInfo.playerInfo.stamina = stamina + staminaRegen > maxStamina ? maxStamina : stamina + staminaRegen;
    }

    private void StartEnemyAction()
    {
        //Add enemy attack AI based on enemy you're fighting in CombatInfo
        gameInfo.playerInfo.health = gameInfo.playerInfo.health - 20 < 0 ? 0 : gameInfo.playerInfo.health - 20;

        descriptionText.GetComponent<TextMeshProUGUI>().text = "Enemy Dealt " + 20 + " Damage!  <sprite index=0>";
        descriptionText.SetActive(true);

        descriptionText.GetComponent<Button>().Select();

        playerTurn = true;
    }

    public void SelectAttack()
    {
        //Do what happens when you select the Attack button
        //Select AttackSelection screen
        mainSelection.SetActive(false);
        attackSelection.SetActive(true);

        lightAttackButton.GetComponent<Button>().Select();
    }

    public void SelectLightAttack()
    {
        //Do what happens when you select the Light Attack button
        //Do Light Attack Action
        int enemyHealth = gameInfo.combatInfo.enemyHealth;
        int stamina = gameInfo.playerInfo.stamina;
        int staminaUsed = 10;
        int damageDealt = 10;

        attackSelection.SetActive(false);

        if (stamina < staminaUsed)
        {
            actionNotPossible = true;

            descriptionText.GetComponent<TextMeshProUGUI>().text = "Not Enough Stamina to do a Light Attack!  <sprite index=0>";
        }
        else
        {
            gameInfo.playerInfo.stamina = stamina - staminaUsed;
            gameInfo.combatInfo.enemyHealth = enemyHealth - damageDealt < 0 ? 0 : enemyHealth - damageDealt;

            descriptionText.GetComponent<TextMeshProUGUI>().text = "You Dealt " + damageDealt + " Damage to the Enemy!  <sprite index=0>";

            playerTurn = false;
        }

        descriptionText.SetActive(true);

        descriptionText.GetComponent<Button>().Select();
    }

    public void SelectHeavyAttack()
    {
        //Do what happens when you select the Heavy Attack button
        //Do Heavy Attack Action
        int enemyHealth = gameInfo.combatInfo.enemyHealth;
        int stamina = gameInfo.playerInfo.stamina;
        int staminaUsed = 30;
        int damageDealt = 30;

        attackSelection.SetActive(false);

        if (stamina < staminaUsed)
        {
            actionNotPossible = true;

            descriptionText.GetComponent<TextMeshProUGUI>().text = "Not Enough Stamina to do a Heavy Attack!  <sprite index=0>";
        }
        else
        {
            gameInfo.playerInfo.stamina = stamina - staminaUsed;
            gameInfo.combatInfo.enemyHealth = enemyHealth - damageDealt < 0 ? 0 : enemyHealth - damageDealt;

            descriptionText.GetComponent<TextMeshProUGUI>().text = "You Dealt " + damageDealt + " Damage to the Enemy!  <sprite index=0>";

            playerTurn = false;
        }

        descriptionText.SetActive(true);

        descriptionText.GetComponent<Button>().Select();
    }

    public void SelectSpecial()
    {
        //Do what happens when you select the Special button
        //Select SpecialSelection screen
        mainSelection.SetActive(false);
        specialSelection.SetActive(true);

        superPunchButton.GetComponent<Button>().Select();
    }

    public void SelectSuperPunch()
    {
        //Do what happens when you select the Super Punch button
        //Do Super Punch Action
        int enemyHealth = gameInfo.combatInfo.enemyHealth;
        int stamina = gameInfo.playerInfo.stamina;
        int staminaUsed = 50;
        int damageDealt = 50;

        specialSelection.SetActive(false);

        if (stamina < staminaUsed)
        {
            actionNotPossible = true;

            descriptionText.GetComponent<TextMeshProUGUI>().text = "Not Enough Stamina to do a Super Punch!  <sprite index=0>";
        }
        else
        {
            gameInfo.playerInfo.stamina = stamina - staminaUsed;
            gameInfo.combatInfo.enemyHealth = enemyHealth - damageDealt < 0 ? 0 : enemyHealth - damageDealt;

            descriptionText.GetComponent<TextMeshProUGUI>().text = "You Dealt " + damageDealt + " Damage to the Enemy!  <sprite index=0>";

            playerTurn = false;
        }

        descriptionText.SetActive(true);

        descriptionText.GetComponent<Button>().Select();
    }

    public void SelectDragonsBreath()
    {
        //Do what happens when you select the Dragon's Breath button
        //Do Dragon's Breath Action
        int enemyHealth = gameInfo.combatInfo.enemyHealth;
        int stamina = gameInfo.playerInfo.stamina;
        int staminaUsed = 60;
        int damageDealt = 60;

        specialSelection.SetActive(false);

        if (stamina < staminaUsed)
        {
            actionNotPossible = true;

            descriptionText.GetComponent<TextMeshProUGUI>().text = "Not Enough Stamina to do Dragon's Breath!  <sprite index=0>";
        }
        else
        {
            gameInfo.playerInfo.stamina = stamina - staminaUsed;
            gameInfo.combatInfo.enemyHealth = enemyHealth - damageDealt < 0 ? 0 : enemyHealth - damageDealt;

            descriptionText.GetComponent<TextMeshProUGUI>().text = "You Dealt " + damageDealt + " Damage to the Enemy!  <sprite index=0>";

            playerTurn = false;
        }

        descriptionText.SetActive(true);

        descriptionText.GetComponent<Button>().Select();
    }

    public void SelectSkipTurn()
    {
        mainSelection.SetActive(false);

        //Regen Stamina here. Could also allow passive Stamina regen? Not sure
        RegenStamina(30);

        descriptionText.GetComponent<TextMeshProUGUI>().text = "You Recovered " + 30 + " Stamina!  <sprite index=0>";
        descriptionText.SetActive(true);

        descriptionText.GetComponent<Button>().Select();

        skipTurn = true;
        playerTurn = false;
    }

    public void SelectHealAndRun()
    {
        //Do what happens when you select the Heal/Run button
        //Select Heal/Run screen
        mainSelection.SetActive(false);
        healAndRunSelection.SetActive(true);

        healButton.GetComponent<Button>().Select();
    }

    public void SelectHeal()
    {
        //Select Heal
        if (gameInfo.playerInfo.stamina < 40)
        {
            actionNotPossible = true;

            healAndRunSelection.SetActive(false);

            descriptionText.GetComponent<TextMeshProUGUI>().text = "Not Enough Stamina to Heal!  <sprite index=0>";
            descriptionText.SetActive(true);

            descriptionText.GetComponent<Button>().Select();
        }
        else if(gameInfo.playerInfo.health == gameInfo.playerInfo.maxHealth)
        {
            actionNotPossible = true;

            healAndRunSelection.SetActive(false);

            descriptionText.GetComponent<TextMeshProUGUI>().text = "You're Already at Full Health!  <sprite index=0>";
            descriptionText.SetActive(true);

            descriptionText.GetComponent<Button>().Select();
        }
        else
        {
            //Do what happens when you select the Heal button
            gameInfo.playerInfo.stamina -= 40;

            int health = gameInfo.playerInfo.health;
            int maxHealth = gameInfo.playerInfo.maxHealth;
            int healthRecovered = health + 30 > maxHealth ? maxHealth - health : 30;
            gameInfo.playerInfo.health = health + healthRecovered;

            healAndRunSelection.SetActive(false);

            descriptionText.GetComponent<TextMeshProUGUI>().text = "You Recovered " + healthRecovered + " Health!  <sprite index=0>";
            descriptionText.SetActive(true);

            descriptionText.GetComponent<Button>().Select();

            playerTurn = false;
        }
    }

    public void SelectRunAway()
    {
        //Do what happens when you select the Run Away button
        if(gameInfo.playerInfo.stamina < 50)
        {
            actionNotPossible = true;

            healAndRunSelection.SetActive(false);

            descriptionText.GetComponent<TextMeshProUGUI>().text = "Not Enough Stamina to Run Away!  <sprite index=0>";
            descriptionText.SetActive(true);

            descriptionText.GetComponent<Button>().Select();
        }
        else
        {
            //TODO: Random Chance of escaping BASED ON DEXSTAT
            gameInfo.playerInfo.stamina -= 50;

            float randomEscape = Random.Range(0f, 1f);
            if(randomEscape > 0.5f)
            {
                healAndRunSelection.SetActive(false);

                descriptionText.GetComponent<TextMeshProUGUI>().text = "Got Away Safely!  <sprite index=0>";
                descriptionText.SetActive(true);

                descriptionText.GetComponent<Button>().Select();

                runAway = true;
            }
            else
            {
                actionNotPossible = true;

                healAndRunSelection.SetActive(false);

                descriptionText.GetComponent<TextMeshProUGUI>().text = "Couldn't Escape!  <sprite index=0>";
                descriptionText.SetActive(true);

                descriptionText.GetComponent<Button>().Select();

                playerTurn = false;
            }
        }
    }
}
