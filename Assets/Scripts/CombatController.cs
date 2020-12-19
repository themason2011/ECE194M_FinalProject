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

    public Sprite backgroundDesert;
    public Sprite backgroundLightForest;
    public Sprite backgroundDenseForest;

    public Sprite alien1;
    public Sprite alien2;

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

    public AudioSource audioSource;

    public AudioClip alienAttack;
    public AudioClip alienHit;
    public AudioClip alienHurt;
    public AudioClip missAttack;
    public AudioClip playerHit;
    public AudioClip playerHurt;
    public AudioClip dragonsBreathHit;
    public AudioClip superPunchSwing;
    public AudioClip superPunchHit;
    public AudioClip healingSound;
    public AudioClip runAwaySound;
    public AudioClip battleWon;
    public AudioClip battleLost;
    public AudioClip playerDeath;

    private GameInfo gameInfo;
    private GameObject playerUI;
    private GameObject enemyUI;
    private GameObject backgroundImage;
    private GameObject enemyImage;
    private bool playerTurn = true;
    private bool combatOver = false;
    private bool playerWon = false;
    private bool combatOverTextDisplayed = false;
    private bool actionNotPossible = false;
    private bool runAway = false;
    private bool skipTurn = false;
    private bool playerActionsLocked = false;

    void Awake()
    {
        gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();
        playerUI = GameObject.Find("PlayerShakeContainer");
        enemyUI = GameObject.Find("EnemyShakeContainer");
        backgroundImage = GameObject.Find("BackgroundImage");
        enemyImage = GameObject.Find("EnemyImage");

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
                gameInfo.combatInfo = new CombatInfo("Alien1", 75, 3, 3, 3, 100);
                enemyImage.GetComponent<Image>().sprite = alien1;
                break;
            case 1:
                gameInfo.combatInfo = new CombatInfo("Alien2", 100, 5, 5, 5, 150);
                enemyImage.GetComponent<Image>().sprite = alien2;
                break;
        }
        SetBackgroundImage();
    }

    private void SetBackgroundImage()
    {
        if(gameInfo.currentTileType == "Desert")
        {
            backgroundImage.GetComponent<Image>().sprite = backgroundDesert;
            backgroundImage.GetComponent<Image>().SetNativeSize();
        }
        else if (gameInfo.currentTileType == "LightForest")
        {
            backgroundImage.GetComponent<Image>().sprite = backgroundLightForest;
        }
        else if (gameInfo.currentTileType == "DenseForest")
        {
            backgroundImage.GetComponent<Image>().sprite = backgroundDenseForest;
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
            if(!playerActionsLocked)
            {
                //If Combat is still going
                if (!combatOver)
                {
                    //If play was just handed over to the player by the enemy or player did an invalid move
                    if (playerTurn)
                    {
                        //If player did not choose run away option or tried and did not succeed in running away
                        if (!runAway)
                        {
                            descriptionText.SetActive(false);
                            mainSelection.SetActive(true);

                            attackButton.GetComponent<Button>().Select();

                            if (!actionNotPossible)
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
                            StartCoroutine(EndBattleSequence(true));

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
                            StartCoroutine(EndBattleSequence(false));
                            
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

        StartCoroutine(EnemyAttackFX(true));

        descriptionText.GetComponent<TextMeshProUGUI>().text = "Enemy Dealt " + 20 + " Damage!  <sprite index=0>";
        descriptionText.SetActive(true);

        descriptionText.GetComponent<Button>().Select();

        playerTurn = true;
    }

    private IEnumerator EndBattleSequence(bool playerWon)
    {
        playerActionsLocked = true;

        Camera.main.GetComponent<AudioSource>().Stop();
        if (playerWon)
        {
            audioSource.PlayOneShot(battleWon);
        }
        else
        {
            audioSource.PlayOneShot(battleLost);
            audioSource.PlayOneShot(playerDeath);
        }

        yield return new WaitForSeconds(2.4f);

        playerActionsLocked = false;
    }

    private IEnumerator EnemyAttackFX(bool attackHit)
    {
        playerActionsLocked = true;

        audioSource.PlayOneShot(alienAttack,0.7f);
        if (attackHit)
        {
            yield return new WaitForSeconds(1);
            audioSource.PlayOneShot(alienHit,0.7f);
            yield return new WaitForSeconds(0.6f);
            audioSource.PlayOneShot(playerHit);
            StartCoroutine(PlayerShake(0.15f, 25f));
            audioSource.PlayOneShot(playerHurt,1.5f);
        }
        else
        {
            yield return new WaitForSeconds(1);
            audioSource.PlayOneShot(missAttack);
        }

        //Wait for Shake/Audio to finish
        yield return new WaitForSeconds(0.5f);

        playerActionsLocked = false;
    }

    private IEnumerator PlayerAttackFX(bool attackHit, int attackNumber)
    {
        playerActionsLocked = true;

        if (attackHit)
        {
            if(attackNumber == 0)
            {
                //Light Attack Hit
                audioSource.PlayOneShot(playerHit,1.1f);
            }
            else if (attackNumber == 1)
            {
                //Heavy Attack is a louder version of the Light attack
                audioSource.PlayOneShot(playerHit, 1.5f);
            }
            StartCoroutine(EnemyShake(0.15f, 25f));
            yield return new WaitForSeconds(0.2f);
            audioSource.PlayOneShot(alienHurt);
        }
        else
        {
            audioSource.PlayOneShot(missAttack);
        }

        //Wait for Shake/Audio to finish
        yield return new WaitForSeconds(0.65f);

        playerActionsLocked = false;
    }

    private IEnumerator PlayerSpecialFX(bool attackHit, int specialAttackNumber)
    {
        playerActionsLocked = true;

        //Super Punch
        if (specialAttackNumber == 0)
        {
            audioSource.PlayOneShot(superPunchSwing);
            if (attackHit)
            {
                yield return new WaitForSeconds(0.6f);
                audioSource.PlayOneShot(superPunchHit, 1.3f);
                yield return new WaitForSeconds(0.15f);
                audioSource.PlayOneShot(alienHurt);
                StartCoroutine(EnemyShake(0.15f, 25f));
                //Wait for Shake/Audio to finish
                yield return new WaitForSeconds(1.2f);
            }
            else
            {
                yield return new WaitForSeconds(1);
                audioSource.PlayOneShot(missAttack);
            }
        }
        //Dragon's Breath
        else if(specialAttackNumber == 1)
        {
            if(attackHit)
            {
                yield return new WaitForSeconds(0.1f);
                audioSource.PlayOneShot(dragonsBreathHit);
                yield return new WaitForSeconds(0.15f);
                audioSource.PlayOneShot(alienHurt);
                StartCoroutine(EnemyShake(0.15f, 25f));
                //Wait for Shake/Audio to finish
                yield return new WaitForSeconds(1.2f);
            }
            else
            {
                //TODO: Placeholder; Doesn't sound like a fireball missing
                audioSource.PlayOneShot(missAttack);
            }
        }

        playerActionsLocked = false;
    }

    private IEnumerator PlayerHealFX()
    {
        playerActionsLocked = true;

        audioSource.PlayOneShot(healingSound, 1.25f);

        //Wait for Audio to finish
        yield return new WaitForSeconds(0.75f);

        playerActionsLocked = false;
    }

    private IEnumerator PlayerRunAwayFX()
    {
        playerActionsLocked = true;

        audioSource.PlayOneShot(runAwaySound, 1.5f);

        //Wait for Audio to finish
        yield return new WaitForSeconds(1.5f);

        playerActionsLocked = false;
    }

    private IEnumerator PlayerShake(float duration, float magnitude)
    {
        Vector3 origPlayerPos = playerUI.transform.position;
        Vector3 origBackgroundPos = backgroundImage.transform.position;
        float elapsed = 0f;

        float shakeAmount = magnitude;
        float shareOffset = 0;
        float lerp = 0;

        while (elapsed < duration)
        {
            lerp = Mathf.PingPong(elapsed + duration/8, duration/4) / (duration/4);
            shareOffset = Mathf.Lerp(-shakeAmount, shakeAmount, lerp);
            playerUI.transform.position = new Vector3(shareOffset+origPlayerPos.x, origPlayerPos.y, 0);
            backgroundImage.transform.position = new Vector3(shareOffset + origBackgroundPos.x, origBackgroundPos.y, 0);
            elapsed += Time.deltaTime;
            yield return 0;
        }

        playerUI.transform.position = origPlayerPos;
        backgroundImage.transform.position = origBackgroundPos;
    }

    private IEnumerator EnemyShake(float duration, float magnitude)
    {
        Vector3 origPosition = enemyUI.transform.position;
        float elapsed = 0f;

        float shakeAmount = magnitude;
        float shareOffset = 0;
        float lerp = 0;

        while (elapsed < duration)
        {
            lerp = Mathf.PingPong(elapsed + duration / 8, duration / 4) / (duration / 4);
            shareOffset = Mathf.Lerp(-shakeAmount, shakeAmount, lerp);
            enemyUI.transform.position = new Vector3(shareOffset + origPosition.x, origPosition.y, 0);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        enemyUI.transform.position = origPosition;
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

            StartCoroutine(PlayerAttackFX(true, 0));

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

            StartCoroutine(PlayerAttackFX(true, 1));

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

            StartCoroutine(PlayerSpecialFX(true, 0));

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

            StartCoroutine(PlayerSpecialFX(true, 1));

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

            audioSource.PlayOneShot(healingSound,1.25f);

            StartCoroutine(PlayerHealFX());

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

                StartCoroutine(PlayerRunAwayFX());

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
