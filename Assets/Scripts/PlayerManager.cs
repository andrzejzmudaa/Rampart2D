using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public Game_Phase_Controller_Script GamePhaseController;
    public KeyCode keyUP;
    public KeyCode keyDOWN;
    public KeyCode keyLEFT;
    public KeyCode keyRIGHT;
    public KeyCode keyPUT;
    public KeyCode keyROTATE;
    public Tilemap playerMap;
    private Tilemap playerMapCloneForPuttingWall;
    private Tilemap plyerMapCloneForCastleBackground;
    public Color playerColor;
    public TileData playableTiles;
    public TileData castleTiles;
    public brick_script brickPrefab;
    public cannon_transparent_prefab_script cannon_transparent_prefab;
    public cannon_prefab_script cannon_prefab;
    public aimScript aim_Prefab;
    private BoundsInt mapBordersCounted;
    private Dictionary<Vector2Int, rampartTile> playerTiles;
    private brick_script brickPrefabInstance;
    private aimScript aim_Prefab_Instance;
    private int playerCannonsToPut;
    private TileBase lastPlayableTile;
    private bool cannonsToPutCountedInFirstCycleOfPutCannonsPhase;
    private cannon_transparent_prefab_script cannonTransparentPlayerInstance;
    private List<cannon_prefab_script> cannonsList;


    cannon_prefab_script debugCannonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //brickPrefabInstance = brickPrefab;
        cannonsList = new List<cannon_prefab_script>();
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

        mapBordersCounted = initializePlayerTilesDictionary();


        brickPrefabInstance.setInitialPositionInMiddle(mapBordersCounted, playerMap);
        brickPrefabInstance.setRandomBrickFromList(playerMapCloneForPuttingWall, playerTiles);
        //brickPrefabInstance.setPlayerColor(playerColor);
        //playerMap.EditorPreviewFloodFill(Vector3Int.zero, new Tile());
        //playerMap.BoxFill(Vector3Int.zero, new Tile(), playerMap.cellBounds.min.x, playerMap.cellBounds.min.y, playerMap.cellBounds.max.x, playerMap.cellBounds.max.y);

        playerMapCloneForPuttingWall.ClearAllTiles();
        playerCannonsToPut = budzyn.processMap2D(playerTiles, mapBordersCounted, playerMap , playerColor);


        cannonTransparentPlayerInstance = GameObject.Instantiate<cannon_transparent_prefab_script>(cannon_transparent_prefab);
        cannonTransparentPlayerInstance.executeAssigingInternalTextReference();
        cannonTransparentPlayerInstance.setCannonMaterialColor(playerColor);
        cannonTransparentPlayerInstance.setInitialPositionInMiddle(playerMap);


        aim_Prefab_Instance = GameObject.Instantiate<aimScript>(aim_Prefab);
        aim_Prefab_Instance.setAimMaterialColor(playerColor);
        aim_Prefab_Instance.setInitialPositionInMiddle(playerMap);


        //Debug.Log("playerMap.cellBounds.min.x " + playerMap.cellBounds.min.x);
        //Debug.Log("playerMap.cellBounds.max.x " + playerMap.cellBounds.max.x);
        //Debug.Log("playerMap.cellBounds.min.y " + playerMap.cellBounds.min.y);
        //Debug.Log("playerMap.cellBounds.max.y " + playerMap.cellBounds.max.y);
    }



            // Update is called once per frame
    void Update()
    {
        playerPhaseGameControllor(GamePhaseController.getGameStateStatus());

    }

    private void playerPhaseGameControllor(Game_Phase_Controller_Script.phase_Enum _gamePhaseStateStatus)
    {
        switch (_gamePhaseStateStatus)
        {
            case Game_Phase_Controller_Script.phase_Enum.GameNotStarted:
                cannonTransparentPlayerInstance.gameObject.SetActive(false);
                brickPrefabInstance.gameObject.SetActive(false);
                aim_Prefab_Instance.gameObject.SetActive(false);
                break;
            case Game_Phase_Controller_Script.phase_Enum.Pause:
                break;
            case Game_Phase_Controller_Script.phase_Enum.Build:
                cannonTransparentPlayerInstance.gameObject.SetActive(false);
                brickPrefabInstance.gameObject.SetActive(true);
                aim_Prefab_Instance.gameObject.SetActive(false);
                InputBrickControl();
                if (cannonsToPutCountedInFirstCycleOfPutCannonsPhase)
                    cannonsToPutCountedInFirstCycleOfPutCannonsPhase = false;
                break;
            case Game_Phase_Controller_Script.phase_Enum.putCannons:
                if (!cannonsToPutCountedInFirstCycleOfPutCannonsPhase)
                {
                    playerCannonsToPut += playerCannonsToPut = budzyn.processMap2D(playerTiles, mapBordersCounted, playerMap, playerColor);
                    if (playerCannonsToPut > 99)
                        playerCannonsToPut = 99;
                    cannonsToPutCountedInFirstCycleOfPutCannonsPhase = true;
                }
                cannonTransparentPlayerInstance.updatecannonAmmountToPutText(playerCannonsToPut);
                cannonTransparentPlayerInstance.gameObject.SetActive(true);
                brickPrefabInstance.gameObject.SetActive(false);
                aim_Prefab_Instance.gameObject.SetActive(false);
                InputCannonPutControl();
                break;
            case Game_Phase_Controller_Script.phase_Enum.Battle:
                cannonTransparentPlayerInstance.gameObject.SetActive(false);
                brickPrefabInstance.gameObject.SetActive(false);
                aim_Prefab_Instance.gameObject.SetActive(true);
                InputAimControl();
                break;
        }
    }

    private void InputCannonPutControl()
    {
        if (Input.GetKeyDown(keyUP))
        {
            cannonTransparentPlayerInstance.moveCannonUp(playerMap);
        }
        if (Input.GetKeyDown(keyDOWN))
        {
            cannonTransparentPlayerInstance.moveCannonDown(playerMap);
        }
        if (Input.GetKeyDown(keyLEFT))
        {
            cannonTransparentPlayerInstance.moveCannonLeft(playerMap);
        }
        if (Input.GetKeyDown(keyRIGHT))
        {
            cannonTransparentPlayerInstance.moveCannonRight(playerMap);
        }
        if (Input.GetKeyDown(keyPUT))
        {
            if (playerCannonsToPut > 0)
            {
                if(cannonTransparentPlayerInstance.checkIfPutCannonOnPlacetPossibleAndSet(playerMapCloneForPuttingWall, playerTiles, this))
                    playerCannonsToPut--;
            }
        }





    }

    private void InputAimControl()
    {

        if (Input.GetKey(keyUP))
        {
            aim_Prefab_Instance.moveAimUp();
            refreshCannonsPossitions();
        }
        if (Input.GetKey(keyDOWN))
        {
            aim_Prefab_Instance.moveAimDown();
            refreshCannonsPossitions();
        }
        if (Input.GetKey(keyLEFT))
        {
            aim_Prefab_Instance.moveAimLeft();
            refreshCannonsPossitions();
        }
        if (Input.GetKey(keyRIGHT))
        {
            aim_Prefab_Instance.moveAimRight();
            refreshCannonsPossitions();
        }
        if (Input.GetKeyDown(keyPUT))
        {
            cannonShoot();
        }
        void refreshCannonsPossitions()
        {
            foreach (cannon_prefab_script singleCannon in cannonsList)
                singleCannon.rotateToPlayerAim(aim_Prefab_Instance.transform.position);
        }
        void cannonShoot()
        {
            foreach (cannon_prefab_script singleCannon in cannonsList)
            {
                singleCannon.rotateToPlayerAim(aim_Prefab_Instance.transform.position);
                singleCannon.shootCanonBall(aim_Prefab_Instance.transform.position);

            }

        }

    }

    private void InputBrickControl()
    {
        if (Input.GetKeyDown(keyROTATE))
        {
            brickPrefabInstance.rotateBrick(playerMapCloneForPuttingWall, playerTiles);
        }
        if (Input.GetKeyDown(keyUP))
        {
            brickPrefabInstance.moveBrickUp(playerMapCloneForPuttingWall, playerTiles);
        }
        if (Input.GetKeyDown(keyDOWN))
        {
            brickPrefabInstance.moveBrickDown(playerMapCloneForPuttingWall, playerTiles);
        }
        if (Input.GetKeyDown(keyLEFT))
        {
            brickPrefabInstance.moveBrickLeft(playerMapCloneForPuttingWall, playerTiles);
        }
        if (Input.GetKeyDown(keyRIGHT))
        {
            brickPrefabInstance.moveBrickRight(playerMapCloneForPuttingWall, playerTiles);
        }
        if (Input.GetKeyDown(keyPUT))
        {
            if (brickPrefabInstance.setBricksOccupiedOnMap(playerMapCloneForPuttingWall, playerTiles))
            {
                budzyn.processMap2D(playerTiles, mapBordersCounted , playerMap , playerColor);
                //playerMapCloneForPuttingWall.RefreshAllTiles();
                brickPrefabInstance.setRandomBrickFromList(playerMapCloneForPuttingWall, playerTiles);
                
            }
        }
    }


    public void verifyPlayerFieldHit(Vector3Int _targetPosInt)
    {
        Vector3Int wallBrickPos = playerMap.WorldToCell(_targetPosInt);
        rampartTile tempTile;
        //Debug.Log("MapPosition :" + wallBrickPos);
        playerTiles.TryGetValue(new Vector2Int(wallBrickPos.x, wallBrickPos.y),out tempTile);
        //Debug.Log("PLayer Tile :" + tempTile);
        if (tempTile == null)
        {
            //Debug.Log("Null found :" + wallBrickPos);
            return;
        }
        if (tempTile.isOccupiedByWall)
        {
            tempTile.isOccupiedByWall = false;
            playerMapCloneForPuttingWall.SetTile(wallBrickPos, new Tile()) ;
            budzyn.processMap2D(playerTiles, mapBordersCounted, playerMap, playerColor);
        }
        if (tempTile.isOccupiedByCannonField)
        {
            var list = playerTiles.Where(kv => kv.Value.isOccupiedByCannonField && kv.Value.parentCannon == tempTile.parentCannon).Select(kv => kv.Value).ToArray();
            cannonsList.Remove(tempTile.parentCannon);
            cannon_prefab_script tempCannonRef = tempTile.parentCannon;
            foreach (rampartTile singleTile in list)
            {
                singleTile.isOccupiedByCannonField = false;
                singleTile.parentCannon = null;
            }
            Destroy(tempCannonRef.gameObject);
        }
    }

    public cannon_prefab_script instantiateCannonPrefab(Vector2Int _cannonPlace)
    {
        cannon_prefab_script tempCannonRef = Object.Instantiate<cannon_prefab_script>(cannon_prefab, new Vector3(_cannonPlace.x, _cannonPlace.y, this.transform.position.z), this.transform.rotation, this.transform);
        cannonsList.Add(tempCannonRef);
        return tempCannonRef;
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
                    playerTiles.Add(tempVector2int, new rampartTile(tempVector2int, ref playerMapCloneForPuttingWall, isPlayable , isCastle));
                
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
