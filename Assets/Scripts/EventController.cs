using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventController : MonoBehaviour
{
    private GameInfo gameInfo;
    private GameObject playerController;

    void Start()
    {
        gameInfo = GameObject.Find("GameInfo").GetComponent<GameInfo>();
        playerController = GameObject.Find("PlayerController");

        LoadEvent(gameInfo.scenarioNumber);
    }

    private void LoadEvent(int eventNumber)
    {
        //Load EventInfo based on eventNumber
        //Events will change when more are added, they get added here.

        //Events are text-based choices (might remove this feature if time does not allow. Focus more on combat first)
        //Load event variables
        switch (eventNumber)
        {
            case 0:
                gameInfo.eventInfo = new EventInfo();
                break;
            case 1:
                gameInfo.eventInfo = new EventInfo();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Run the event in here
    }
}
