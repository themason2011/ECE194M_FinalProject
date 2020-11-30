using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    WorldMapInput worldMapInput;

    private Grid map;

    private GameObject worldMapController;
    private GameObject gameController;

    private Vector3 destination;

    private void Awake()
    {
        worldMapInput = new WorldMapInput();
    }

    private void OnEnable()
    {
        worldMapInput.Enable();
    }

    private void OnDisable()
    {
        worldMapInput.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
        worldMapController = GameObject.Find("WorldMapController");
        gameController = GameObject.Find("GameController");
        map = GameObject.Find("Grid").GetComponent<Grid>();
        worldMapInput.Mouse.MouseClick.performed += _ => MouseClick();
    }

    private void MouseClick()
    {
        Vector2 mousePosition = worldMapInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int gridPosition = map.WorldToCell(mousePosition);
        //Check if there is a tile where player is clicking and if the tile you want to move to is adjacent to player's current tile
        if (map.GetComponentInChildren<Tilemap>().HasTile(gridPosition) && Vector2.Distance(map.GetCellCenterWorld(gridPosition),transform.position) <= 0.85f)
        {
            destination = map.GetCellCenterWorld(gridPosition);
            transform.position = destination;
            gameController.GetComponent<GameController>().worldMapPosition = transform.position;
            worldMapController.GetComponent<WorldMapController>().RollScenario();
        }
    }
}
