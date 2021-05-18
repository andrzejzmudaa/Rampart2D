using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerOneManager : MonoBehaviour
{
    public Tilemap playerMap;
    private Tilemap playerMapCloneForPuttingWall;
    private Tilemap plyerMapCloneForCastleBackground;
    public Color playerColor;
    public TileData playableTiles;
    public TileData castleTiles;
    public brick_script brickPrefab;
    private float mapGridSizeX;
    private float mapGridSizeY;
    private BoundsInt mapBordersCounted;
    private GameObject brickref;
    private Dictionary<Vector2Int, rampartTile> playerTiles;
    private brick_script brickPrefabInstance;
    private TileBase lastPlayableTile;




    // Start is called before the first frame update
    void Start()
    {
        //brickPrefabInstance = brickPrefab;
        brickPrefabInstance = GameObject.Instantiate<brick_script>(brickPrefab);
        brickPrefabInstance.InitializazeFunction();
        brickPrefabInstance.setPlayerColor(playerColor);
        playerMap.CompressBounds();
        playerMapCloneForPuttingWall = Object.Instantiate(playerMap, new Vector3(playerMap.transform.position.x, playerMap.transform.position.y, playerMap.transform.position.z - 1), playerMap.transform.rotation, playerMap.transform);
        playerMapCloneForPuttingWall.gameObject.name = "playerMapCloneForPuttingWall";
        plyerMapCloneForCastleBackground = Object.Instantiate(playerMapCloneForPuttingWall, new Vector3(playerMap.transform.position.x, playerMap.transform.position.y, playerMap.transform.position.z + 2), playerMap.transform.rotation, playerMap.transform);
        plyerMapCloneForCastleBackground.gameObject.name = "plyerMapCloneForCastleBackground";
        playerMapCloneForPuttingWall.color = playerColor;
        plyerMapCloneForCastleBackground.ClearAllTiles();
        //plyerMapCloneForCastleBackground.RefreshAllTiles();

        playerTiles = new Dictionary<Vector2Int, rampartTile>();
        mapGridSizeX = playerMap.layoutGrid.cellSize.x;
        mapGridSizeY = playerMap.layoutGrid.cellSize.y;

        TileBase[] allTiles = playerMap.GetTilesBlock(playerMap.cellBounds);

        mapBordersCounted = initializePlayerTilesDictionary();


        brickPrefabInstance.setInitialPositionInMiddle(mapBordersCounted, playerMap);
        brickPrefabInstance.setRandomBrickFromList(playerMapCloneForPuttingWall, playerTiles);
        //brickPrefabInstance.setPlayerColor(playerColor);
        //playerMap.EditorPreviewFloodFill(Vector3Int.zero, new Tile());
        //playerMap.BoxFill(Vector3Int.zero, new Tile(), playerMap.cellBounds.min.x, playerMap.cellBounds.min.y, playerMap.cellBounds.max.x, playerMap.cellBounds.max.y);


        playerMapCloneForPuttingWall.ClearAllTiles();
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
            brickPrefabInstance.rotateBrick(playerMapCloneForPuttingWall, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            brickPrefabInstance.moveBrickUp(playerMapCloneForPuttingWall, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            brickPrefabInstance.moveBrickDown(playerMapCloneForPuttingWall, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            brickPrefabInstance.moveBrickLeft(playerMapCloneForPuttingWall, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            brickPrefabInstance.moveBrickRight(playerMapCloneForPuttingWall, playerTiles);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (brickPrefabInstance.setBricksOccupiedOnMap(playerMapCloneForPuttingWall, playerTiles))
            {
                budzyn.processMap2D(playerTiles, mapBordersCounted , playerMap , playerColor);
                //playerMapCloneForPuttingWall.RefreshAllTiles();
                brickPrefabInstance.setRandomBrickFromList(playerMapCloneForPuttingWall, playerTiles);
                
            }
        }
    }


    public void destroyWallBrick(Vector3Int _wallBrickPos)
    {
        rampartTile tempTile;
        playerTiles.TryGetValue(new Vector2Int(_wallBrickPos.x, _wallBrickPos.y),out tempTile);
        if (tempTile == null)
            return;
        if (tempTile.isOccupied)
        {
            tempTile.isOccupied = false;
            playerMapCloneForPuttingWall.SetTile(_wallBrickPos, new Tile()) ;
            budzyn.processMap2D(playerTiles, mapBordersCounted, playerMap, playerColor);
            
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

                if (x < tempMapMinSizeX)
                        tempMapMinSizeX = x;
                if (x > tempMapMaxSizeX)
                        tempMapMaxSizeX = x;
                if (y < tempMapMinSizeY)
                        tempMapMinSizeY = y;
                if (y > tempMapMaxSizeY)
                        tempMapMaxSizeY = y;

                bool isPlayable = false;
                bool isOccupied = false;
                bool isCastle = false;
                if (tempTile != null) 
                { 
                    foreach (TileBase singleTileBase in playableTiles.playableTiles)
                    {
                        if (tempTile == singleTileBase)
                        {
                            lastPlayableTile = singleTileBase;
                            tempTile.flags = TileFlags.None;
                            playerMap.SetTileFlags(new Vector3Int(x, y, 0), TileFlags.None);
                            isPlayable = true;
                            budzyn.setBackgroundColorAsChess(new Vector2Int(x, y), playerMap);

                        }
                        foreach (TileBase singleCastleTileBase in castleTiles.playableTiles)
                        {
                            if (tempTile == singleCastleTileBase)
                            {
                                isCastle = true;
                                plyerMapCloneForCastleBackground.SetTile(new Vector3Int(x,y,0), lastPlayableTile);
                                budzyn.setBackgroundColorAsChess(new Vector2Int(x,y), plyerMapCloneForCastleBackground);

                            }

                        }
                    }
                }

                    Vector2Int tempVector2int = new Vector2Int(x, y);
                    playerTiles.Add(tempVector2int, new rampartTile(tempVector2int, ref playerMapCloneForPuttingWall, isPlayable ,  isOccupied , isCastle));
                
            }
        }
        BoundsInt tempBound = new BoundsInt();
        tempBound.xMin = tempMapMinSizeX;
        tempBound.xMax = tempMapMaxSizeX;
        tempBound.yMin = tempMapMinSizeY;
        tempBound.yMax = tempMapMaxSizeY;
        playerMap.RefreshAllTiles();
        plyerMapCloneForCastleBackground.CompressBounds();
        return tempBound;

    }


}
