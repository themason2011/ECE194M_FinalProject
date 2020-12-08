﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public int scenarioNumber;
    public Vector3 worldMapPosition;
    public Grid grid;
    public EventInfo eventInfo;
    public CombatInfo combatInfo;
    public PlayerInfo playerInfo;

    public GameObject worldMapControllerPrefab;

    private static GameInfo gameInfoInstance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        //Prevent multiple copies of GameInfo from showing up when reloading the WorldMap Scene
        if(gameInfoInstance == null)
        {
            gameInfoInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        scenarioNumber = 0;
        worldMapPosition = new Vector3(-3.5f, -0.4875f, 0f);
        eventInfo = new EventInfo();
        combatInfo = new CombatInfo();
        playerInfo = new PlayerInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }
}