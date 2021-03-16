using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class brick_script : MonoBehaviour
{
    GameObject thisObject;
    public Sprite borderBrickSprite;
    public Sprite brickSprite;
    public brickTemplatesContainer availableBricks;
    private brickTemplate brickProperties;
    private brickPropertiesClass.angleType angle;
    private bool[,] occupied_matrix4x4;
    private Grid localGrid4x4;
    private Tilemap localTilemap;
    private TilemapRenderer localTilemapRenderer;
    private TileBase localTileBase;
    private TileData localTileData;
    private Tile brickTile;
    private Tile borderTile;
    Tile emptyTile;
    public Material colorMaterial;
    public Material borderMaterial;

    // Start is called before the first frame update
    void Start()
    {
        thisObject = this.gameObject;
        angle = brickPropertiesClass.angleType.A0deg;
        localGrid4x4 = this.gameObject.GetComponent<Grid>();
        localTilemap = this.gameObject.GetComponent<Tilemap>();
        localTilemapRenderer = this.gameObject.GetComponent<TilemapRenderer>();
        emptyTile = new Tile();
        brickTile = new Tile();
        borderTile = new Tile(); 
        brickTile.sprite = brickSprite;
        borderTile.sprite = borderBrickSprite;
        //brickTile.sprite.


        //printBool2dArray(brickProperties.getBoolean2dMatrix());
        //Debug.Log("Dane: "+ getBoolean2dMatrix(brickProperties));
        setRandomBrickFromList();

        



        setTilesAccordingTo2dMatrix(brickProperties.getBoolean2dMatrix(angle));
        



    }

    // Update is called once per frame
    void Update()
    {
        


    }

    

    public void printBool2dArray(bool[,] incommingArray)
    {
        int rowLength = incommingArray.GetLength(0);
        int colLength = incommingArray.GetLength(1);

        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                Debug.Log(string.Format("{0} ", incommingArray[i, j]));
            }
            Debug.Log((Environment.NewLine + Environment.NewLine));
        }

    }

    private void setTilesAccordingTo2dMatrix(bool[,] incommingArray)
    {
        int rowLength = incommingArray.GetLength(0);
        int colLength = incommingArray.GetLength(1);

        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                if(incommingArray[i, j])
                    localTilemap.SetTile(new Vector3Int(-j+1, i-2, 0), borderTile);
                else
                    localTilemap.SetTile(new Vector3Int(-j + 1, i - 2, 0),emptyTile );
            }
        }
        localTilemap.RefreshAllTiles();
    }

    public void rotateBrick(Tilemap _playerMap, Dictionary<Vector2Int, rampartTile> _playerTiles)
    {
        brickPropertiesClass.angleType tempAngle = angle + 1;
        tempAngle = ((brickPropertiesClass.angleType)((int)tempAngle % 4));
        if (checkIfMovementPossible(retriveBrickPosition3dVector(), _playerMap, _playerTiles, tempAngle))
        {
            //Debug.Log("Rotation possible");
            angle++;
            angle = ((brickPropertiesClass.angleType)((int)angle % 4));
            setTilesAccordingTo2dMatrix(brickProperties.getBoolean2dMatrix(angle));
        }
    }

    public void moveBrickUp(Tilemap _playerMap, Dictionary<Vector2Int, rampartTile> _playerTiles)
    {
        if (checkIfMovementPossible((retriveBrickPosition3dVector() + Vector3.up), _playerMap, _playerTiles, angle))
            thisObject.transform.position = new Vector3(thisObject.transform.position.x, thisObject.transform.position.y + 1, thisObject.transform.position.z);
        
    }
    public void moveBrickDown(Tilemap _playerMap, Dictionary<Vector2Int, rampartTile> _playerTiles)
    {
        if (checkIfMovementPossible((retriveBrickPosition3dVector() + Vector3.down), _playerMap, _playerTiles, angle))
            thisObject.transform.position = new Vector3(thisObject.transform.position.x, thisObject.transform.position.y - 1, thisObject.transform.position.z);
    }
    public void moveBrickLeft(Tilemap _playerMap, Dictionary<Vector2Int, rampartTile> _playerTiles)
    {
        if (checkIfMovementPossible((retriveBrickPosition3dVector() + Vector3.left), _playerMap, _playerTiles, angle))
            thisObject.transform.position = new Vector3(thisObject.transform.position.x - 1, thisObject.transform.position.y, thisObject.transform.position.z);
    }
    public void moveBrickRight(Tilemap _playerMap, Dictionary<Vector2Int, rampartTile> _playerTiles)
    {
        if (checkIfMovementPossible((retriveBrickPosition3dVector() + Vector3.right), _playerMap, _playerTiles, angle))
            thisObject.transform.position = new Vector3(thisObject.transform.position.x + 1, thisObject.transform.position.y, thisObject.transform.position.z);
    }

    public bool setBricksOccupiedOnMap(Tilemap _playerMap, Dictionary<Vector2Int, rampartTile> _playerTiles)
    {
        return checkIfPossibleAndSetBrick(retriveBrickPosition3dVector(), _playerMap, _playerTiles, angle);
    }

    public void setRandomBrickFromList()
    {
        int brickRandomNumber = UnityEngine.Random.Range(0, availableBricks.brickTemplates.Length);
        brickProperties = (brickTemplate)availableBricks.brickTemplates.GetValue(brickRandomNumber);
        angle = 0;
        setTilesAccordingTo2dMatrix(brickProperties.getBoolean2dMatrix(angle));
    }


    public bool checkIfMovementPossible(Vector3 _position,Tilemap _playerMap, Dictionary<Vector2Int, rampartTile> _playerTiles, brickPropertiesClass.angleType _angle)
    {
        bool[,] brick2dArray = retriveBrick2dArray(_angle);
        Vector3 brickCenterWorldPosition = _position; 
        Vector3Int cellPosition = _playerMap.WorldToCell(brickCenterWorldPosition);
        int rowLength = brick2dArray.GetLength(0);
        int colLength = brick2dArray.GetLength(1);
        bool isMovementPossible = true;
        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                //Debug.Log("Cell [ " + i + " ][ "+ j + " ] = "  + brick2dArray[i , j]);
                if (brick2dArray[i, j])
                    if (!_playerTiles.TryGetValue(rampartTile.convertVector3IntToVector2Int(new Vector3Int((cellPosition.x + j), cellPosition.y - i, cellPosition.z)), out rampartTile _outTile))
                        isMovementPossible = false;
            }
        }
        return isMovementPossible;
    }

    public bool checkIfPossibleAndSetBrick(Vector3 _position, Tilemap _playerMap, Dictionary<Vector2Int, rampartTile> _playerTiles, brickPropertiesClass.angleType _angle)
    {
        bool[,] brick2dArray = retriveBrick2dArray(_angle);
        Vector3 brickCenterWorldPosition = _position;
        Vector3Int cellPosition = _playerMap.WorldToCell(brickCenterWorldPosition);
        List<rampartTile> tempRampartTileListToModify = new List<rampartTile>();
        int rowLength = brick2dArray.GetLength(0);
        int colLength = brick2dArray.GetLength(1);
        bool isSetBrickPossible = true;
        for (int i = 0; i < rowLength; i++)
        {
            for (int j = 0; j < colLength; j++)
            {
                //Debug.Log("Cell [ " + i + " ][ "+ j + " ] = "  + brick2dArray[i , j]);
                if (brick2dArray[i, j])
                    if (_playerTiles.TryGetValue(rampartTile.convertVector3IntToVector2Int(new Vector3Int((cellPosition.x + j), cellPosition.y - i, cellPosition.z)), out rampartTile _outTile))
                    {
                        if (_outTile.isPlayable && !_outTile.isOccupied)
                            tempRampartTileListToModify.Add(_outTile);
                        else
                            isSetBrickPossible = false;
                    }
                    else
                        isSetBrickPossible = false;
            }
        }
        if (isSetBrickPossible)
        {
            foreach(rampartTile singleTile in tempRampartTileListToModify)
            {
                singleTile.setNewTileBase(brickTile);
            }

        }
        return isSetBrickPossible;

    }



    public bool[,] retriveBrick2dArray(brickPropertiesClass.angleType _angle)
    {
        return brickProperties.getBoolean2dMatrix(_angle);
    }

    public Vector3 retriveBrickPosition3dVector()
    {
        return localTilemap.GetCellCenterWorld(new Vector3Int(1, -2, 0));
    }

    public void setInitialPositionInMiddle(BoundsInt _inputBounds)
    {
        thisObject.transform.position = new Vector3((_inputBounds.xMax - (_inputBounds.xMax - _inputBounds.xMin) / 2), (_inputBounds.yMax - (_inputBounds.yMax - _inputBounds.yMin) / 2), thisObject.transform.position.z);       
    }

    public void setPlayerColor(Color _inputColor)
    {
        Color localInputColor = _inputColor;
        localInputColor.a = 1;
        colorMaterial.color = localInputColor;
        borderMaterial.color = localInputColor;
        Debug.Log("Material is : " + borderMaterial.color);
        //localTilemapRenderer.material = borderMaterial;






    }

}
