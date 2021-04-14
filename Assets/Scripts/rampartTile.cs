using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class rampartTile
{
    Sprite originalSprite;
    TileBase originalBaseTile;
    public readonly bool isPlayable;
    public readonly bool borderTile;
    public readonly bool greyed;
    
    TileBase modifiedTileBase;
    Vector3Int tilePos;

    private Tilemap parrentTilemapRef;
    public tileAlgorithmStatus cellStatus;


    public Sprite OriginalSprite { get => originalSprite; }
    public TileBase OriginalBaseTile { get => originalBaseTile; }
    public bool isOccupied { get; set; }
    //public bool IsOccupied;
    public Vector3Int TilePos { get => tilePos;  }
    //public tileAlgorithmStatus cellStatus { get => cellStatus; set => cellStatus = value; }

    public rampartTile(Vector2Int _tilePos2d,ref Tilemap _parrentTilemap, bool _isPlayable)
    {
        this.tilePos = convertVector2IntToVector3Int(_tilePos2d);
        parrentTilemapRef = _parrentTilemap;
        if (_parrentTilemap.GetTile(tilePos) != null) {
            isPlayable = _isPlayable;
            isOccupied = !_isPlayable;
            this.originalSprite = _parrentTilemap.GetSprite(tilePos);
            originalBaseTile = _parrentTilemap.GetTile(tilePos);
        }
    }

    public void setNewTileBase(TileBase _incomingTileBase)
    {
        modifiedTileBase = _incomingTileBase;
        isOccupied = true;
        parrentTilemapRef.SetTile(tilePos, modifiedTileBase);
    }

    public static Vector2Int convertVector3IntToVector2Int(Vector3Int _incommingVector3Int)
    {
        return new Vector2Int(_incommingVector3Int.x, _incommingVector3Int.y);
    }

    public static Vector3Int convertVector2IntToVector3Int(Vector2Int _incommingVector2Int)
    {
        return new Vector3Int(_incommingVector2Int.x, _incommingVector2Int.y,0);
    }

    public enum tileAlgorithmStatus
    {
        NotDefinedYet,
        NotInternal,
        Wall,
        Internal,
    }
}
