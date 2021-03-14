using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerOneManager : MonoBehaviour
{
    public Tilemap playerMap;
    public Color playerColor;
    public TileData playableTiles;
    public brick_script brickPrefab;
    private float mapGridSizeX;
    private float mapGridSizeY;
    private BoundsInt mapBordersCounted;
    private GameObject brickref;
    private Dictionary<Vector2Int, rampartTile> playerTiles;
    


    // Start is called before the first frame update
    void Start()
    {
        playerMap.CompressBounds();
        playerTiles = new Dictionary<Vector2Int, rampartTile>();
        mapGridSizeX = playerMap.layoutGrid.cellSize.x;
        mapGridSizeY = playerMap.layoutGrid.cellSize.y;

        TileBase[] allTiles = playerMap.GetTilesBlock(playerMap.cellBounds);

        mapBordersCounted = initializePlayerTilesDictionary();
        
        brickPrefab.setInitialPositionInMiddle(mapBordersCounted);
        brickPrefab.setPlayerColor(playerColor);
        //playerMap.EditorPreviewFloodFill(Vector3Int.zero, new Tile());
        //playerMap.BoxFill(Vector3Int.zero, new Tile(), playerMap.cellBounds.min.x, playerMap.cellBounds.min.y, playerMap.cellBounds.max.x, playerMap.cellBounds.max.y);



        budzyn.processMap2D(playerTiles, mapBordersCounted, playerMap);






        //Debug.Log("playerMap.cellBounds.min.x " + playerMap.cellBounds.min.x);
        //Debug.Log("playerMap.cellBounds.max.x " + playerMap.cellBounds.max.x);
        //Debug.Log("playerMap.cellBounds.min.y " + playerMap.cellBounds.min.y);
        //Debug.Log("playerMap.cellBounds.max.y " + playerMap.cellBounds.max.y);


    }



            // Update is called once per frame
            void Update()
    {

        InputControl();

    }

    private void InputControl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            brickPrefab.rotateBrick(playerMap, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            brickPrefab.moveBrickUp(playerMap, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            brickPrefab.moveBrickDown(playerMap, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            brickPrefab.moveBrickLeft(playerMap, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            brickPrefab.moveBrickRight(playerMap, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (brickPrefab.setBricksOccupiedOnMap(playerMap, playerTiles))
            {
                budzyn.processMap2D(playerTiles, mapBordersCounted ,playerMap);
                playerMap.RefreshAllTiles();
                brickPrefab.setRandomBrickFromList();
                
            }
        }
    }


    

    

    private BoundsInt initializePlayerTilesDictionary()
    {
        int tempMapMinSizeX = playerMap.cellBounds.max.x;
        int tempMapMinSizeY = playerMap.cellBounds.max.y;
        int tempMapMaxSizeX = playerMap.cellBounds.min.x;
        int tempMapMaxSizeY = playerMap.cellBounds.min.y;

        for (int x = playerMap.cellBounds.min.x; x < playerMap.cellBounds.max.x; x++)
        {
            for (int y = playerMap.cellBounds.min.y; y < playerMap.cellBounds.max.y; y++)
            {
                TileBase tempTile = playerMap.GetTile(new Vector3Int(x, y, 0));
                if (tempTile != null)
                {
                    if (x < tempMapMinSizeX)
                        tempMapMinSizeX = x;
                    if (x > tempMapMaxSizeX)
                        tempMapMaxSizeX = x;
                    if (y < tempMapMinSizeY)
                        tempMapMinSizeY = y;
                    if (y > tempMapMaxSizeY)
                        tempMapMaxSizeY = y;

                    bool isPlayable = false;
                    foreach (TileBase singleTileBase in playableTiles.playableTiles)
                    {
                        if (tempTile == singleTileBase)
                        {
                            isPlayable = true;
                        }
                    }
                    Vector2Int tempVector2int = new Vector2Int(x, y);
                    playerTiles.Add(tempVector2int, new rampartTile(tempVector2int, ref playerMap, isPlayable));
                }
            }
        }
        BoundsInt tempBound = new BoundsInt();
        tempBound.xMin = tempMapMinSizeX;
        tempBound.xMax = tempMapMaxSizeX;
        tempBound.yMin = tempMapMinSizeY;
        tempBound.yMax = tempMapMaxSizeY;
        return tempBound;

    }
}
