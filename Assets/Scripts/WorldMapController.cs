using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldMapController : MonoBehaviour
{
    private float willScenarioOccur;
    private int chooseScenario;
    private bool isEvent;
    private GameInfo gameInfo;
    private GameObject player;
    private Grid map;

    private static WorldMapController worldMapControllerInstance;

    private void Awake()
    {
        //Prevent multiple copies of WorldMapController from showing up when reloading the WorldMap Scene
        if (worldMapControllerInstance == null)
        {
            worldMapControllerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();
        player = GameObject.Find("Player");
        map = GameObject.Find("Grid").GetComponent<Grid>();
        gameInfo.grid = map;
        chooseScenario = 0;
        isEvent = false;
    }

    public void WinGame()
    {
        Text goldenCityText = GameObject.Find("GoldenCityText").GetComponent<Text>();
        goldenCityText.enabled = true;
        StartCoroutine(WinGameTimer());
    }

    IEnumerator WinGameTimer()
    {
        yield return new WaitForSeconds(2f);
        Application.Quit();
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
                chooseScenario = Random.Range(0, 2);
            }
            gameInfo.GetComponent<GameInfo>().scenarioNumber = chooseScenario;
            if(isEvent)
            {
                map.enabled = false;
                SceneManager.LoadScene("EventScreen");
            }
            else
            {
                map.enabled = false;
                SceneManager.LoadScene("CombatScreen");
            }
        }
    }

    public void EnableWorldMap()
    {
        map.enabled = true;
    }
}
