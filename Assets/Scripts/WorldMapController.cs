using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour
{
    public GameObject playerPrefab;

    private float willScenarioOccur;
    private int chooseScenario;
    private bool isEvent;
    private GameObject gameController;

    void Start()
    {
        gameController = GameObject.Find("GameController");
        chooseScenario = 0;
        isEvent = false;
        Vector3 playerStartPosition = gameController.GetComponent<GameController>().worldMapPosition;
        Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
    }

    public void RollScenario()
    {
        //Decide if a scenario will occur
        willScenarioOccur = Random.Range(0f, 1f);
        if (willScenarioOccur >= 0.75f)
        {
            //Decide if there will be an event scenario or combat scenario
            float isEventScenario = Random.Range(0f, 1f);
            //DEV NOTE: Set to always go to combat for now, change later to enable events if time
            if(isEventScenario > 1.0f)
            {
                isEvent = true;
                //This is the number of event scenarios because an event scenario was chosen
                chooseScenario = Random.Range(0, 2);
            }
            else
            {
                isEvent = false;
                //This is the number of combat scenarios because a combat scenario was chosen
                chooseScenario = Random.Range(0, 3);
            }
            gameController.GetComponent<GameController>().scenarioNumber = chooseScenario;
            if(isEvent)
            {
                SceneManager.LoadScene("EventScreen");
            }
            else
            {
                SceneManager.LoadScene("CombatScreen");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
