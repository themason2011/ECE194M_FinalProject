using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Text playerHealthText;

    private int visionDistance;

    private Grid map;
    private Tilemap moveableTilemap;
    private Tilemap goldenCity;

    private GameObject worldMapController;
    private GameObject gameInfo;
    private Tilemap fogOfWar;

    private bool movementDisabled = false;

    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        worldMapController = GameObject.Find("WorldMapController");
        gameInfo = GameObject.Find("GameInfo");
        fogOfWar = GameObject.Find("Fog").GetComponent<Tilemap>();
        visionDistance = 2;
        UpdateFogOfWar();
        map = GameObject.Find("Grid").GetComponent<Grid>();
        moveableTilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        goldenCity = GameObject.Find("GoldenCity").GetComponent<Tilemap>();

        //Set position for player on load
        transform.position = gameInfo.GetComponent<GameInfo>().worldMapPosition;
        Camera.main.transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y + 0.5f, -10);
    }

    void Update()
    {
        UpdateUI();
        UpdateCameraPosition();

        if (Input.GetMouseButtonDown(0) && !movementDisabled)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);
            float moveDistance = Vector2.Distance(map.GetCellCenterWorld(gridPosition), transform.position);
            //Check if there is a tile where player is clicking and if the tile you want to move to is adjacent to player's current tile.
            //moveDistance having a small range of exclusion is to account for the tiles on the top and bottom, which are slightly closer to the player
            //Than the tiles to the left and right (distances are 0.975 and 1.0, respectively)
            if (moveableTilemap.HasTile(gridPosition) && moveDistance <= 1.0f && !(moveDistance > 0.97f && moveDistance < 0.98f))
            {
                destination = map.GetCellCenterWorld(gridPosition);
                transform.position = destination;
                UpdateFogOfWar();
                gameInfo.GetComponent<GameInfo>().worldMapPosition = transform.position;

                int playerHealth = gameInfo.GetComponent<GameInfo>().playerInfo.health;
                gameInfo.GetComponent<GameInfo>().playerInfo.health = playerHealth + 10 > 100 ? 100 : playerHealth + 10;

                if (goldenCity.HasTile(gridPosition))
                {
                    //You win!
                    movementDisabled = true;
                    worldMapController.GetComponent<WorldMapController>().WinGame();
                }
                else
                {
                    //Haven't found the Golden City yet, Roll to see if a Scenario occurs
                    worldMapController.GetComponent<WorldMapController>().RollScenario();
                }
            }
        }
    }

    private void UpdateUI()
    {
        playerHealthText.text = gameInfo.GetComponent<GameInfo>().playerInfo.health.ToString();
    }

    private void UpdateCameraPosition()
    {
        Camera.main.transform.position = new Vector3(transform.position.x + 0.25f, transform.position.y + 0.5f, -10);
    }

    void UpdateFogOfWar()
    {
        Vector3Int currentPlayerTile = fogOfWar.WorldToCell(transform.position);

        //Clear the surrounding tiles
        for (int x = -visionDistance; x <= visionDistance; x++)
        {
            for (int y = -visionDistance; y <= visionDistance; y++)
            {
                fogOfWar.SetTile(currentPlayerTile + new Vector3Int(x, y, 0), null);
            }

        }
    }
}
