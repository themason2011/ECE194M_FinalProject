using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int scenarioNumber;
    public Vector3 worldMapPosition;
    public EventInfo eventInfo;
    public CombatInfo combatInfo;
    public PlayerInfo playerInfo;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
