using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour
{
    public int scenarioNumber;
    public string currentTileType;
    public Vector3 worldMapPosition;
    public Grid grid;
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
        currentTileType = "DenseForest";
        worldMapPosition = new Vector3(-3.5f, -0.4875f, 0f);
        combatInfo = new CombatInfo();
        playerInfo = new PlayerInfo();
    }
}
