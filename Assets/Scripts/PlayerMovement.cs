using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    //WorldMapInput worldMapInput;
    
    private int visionDistance;

    private Grid map;

    private GameObject worldMapController;
    private GameObject gameInfo;
    private Tilemap fogOfWar;

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

        //Set position for player on load
        transform.position = gameInfo.GetComponent<GameInfo>().worldMapPosition;
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

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);
            float moveDistance = Vector2.Distance(map.GetCellCenterWorld(gridPosition), transform.position);
            //Check if there is a tile where player is clicking and if the tile you want to move to is adjacent to player's current tile.
            //moveDistance having a small range of exclusion is to account for the tiles on the top and bottom, which are slightly closer to the player
            //Than the tiles to the left and right (distances are 0.975 and 1.0, respectively)
            if (map.GetComponentInChildren<Tilemap>().HasTile(gridPosition) && moveDistance <= 1.0f && !(moveDistance > 0.97f && moveDistance < 0.98f))
            {
                destination = map.GetCellCenterWorld(gridPosition);
                transform.position = destination;
                UpdateFogOfWar();
                gameInfo.GetComponent<GameInfo>().worldMapPosition = transform.position;
                worldMapController.GetComponent<WorldMapController>().RollScenario();
            }
        }
    }
}
