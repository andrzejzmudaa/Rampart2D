using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerOneManager : MonoBehaviour
{
    public Tilemap playerMap;
    private Tilemap playerMapClone;
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
        playerMapClone = Object.Instantiate(playerMap, new Vector3(playerMap.transform.position.x, playerMap.transform.position.y, playerMap.transform.position.z - 1), playerMap.transform.rotation, playerMap.transform);
        playerMapClone.color = playerColor;
        playerTiles = new Dictionary<Vector2Int, rampartTile>();
        mapGridSizeX = playerMap.layoutGrid.cellSize.x;
        mapGridSizeY = playerMap.layoutGrid.cellSize.y;

        TileBase[] allTiles = playerMap.GetTilesBlock(playerMap.cellBounds);

        mapBordersCounted = initializePlayerTilesDictionary();
        
        brickPrefab.setInitialPositionInMiddle(mapBordersCounted);
        brickPrefab.setPlayerColor(playerColor);
        //playerMap.EditorPreviewFloodFill(Vector3Int.zero, new Tile());
        //playerMap.BoxFill(Vector3Int.zero, new Tile(), playerMap.cellBounds.min.x, playerMap.cellBounds.min.y, playerMap.cellBounds.max.x, playerMap.cellBounds.max.y);


        playerMapClone.ClearAllTiles();
        budzyn.processMap2D(playerTiles, mapBordersCounted, playerMap , playerColor);






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
            brickPrefab.rotateBrick(playerMapClone, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            brickPrefab.moveBrickUp(playerMapClone, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            brickPrefab.moveBrickDown(playerMapClone, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            brickPrefab.moveBrickLeft(playerMapClone, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            brickPrefab.moveBrickRight(playerMapClone, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (brickPrefab.setBricksOccupiedOnMap(playerMapClone, playerTiles))
            {
                budzyn.processMap2D(playerTiles, mapBordersCounted , playerMap , playerColor);
                playerMapClone.RefreshAllTiles();
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
                Tile tempTile = (Tile)playerMap.GetTile(new Vector3Int(x, y, 0));
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
                            tempTile.flags = TileFlags.None;
                            //playerMap.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
                            isPlayable = true;
                            if ((Mathf.Abs(x) % 2) == (Mathf.Abs(y) % 2))
                            {
                                playerMap.SetColor(new Vector3Int(x, y, 0), new Color(1.0f, 1.0f, 1.0f, 0.8f));
                            }
                            else
                            {
                                playerMap.SetColor(new Vector3Int(x, y, 0), new Color(1.0f, 1.0f, 1.0f, 1.0f));
                            }
                            
                        }
                    }

                    Vector2Int tempVector2int = new Vector2Int(x, y);
                    playerTiles.Add(tempVector2int, new rampartTile(tempVector2int, ref playerMapClone, isPlayable, playerColor, brickPrefab.colorMaterial));
                }
            }
        }
        BoundsInt tempBound = new BoundsInt();
        tempBound.xMin = tempMapMinSizeX;
        tempBound.xMax = tempMapMaxSizeX;
        tempBound.yMin = tempMapMinSizeY;
        tempBound.yMax = tempMapMaxSizeY;
        //playerMap.RefreshAllTiles();
        return tempBound;

    }
}
