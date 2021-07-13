using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class rampartTile
{
    Sprite originalSprite;
    TileBase originalBaseTile;
    public readonly bool isPlayable;
    public readonly bool isCastle;
    public readonly bool borderTile;
    public readonly bool greyed;
    public cannon_prefab_script parentCannon;

    TileBase modifiedTileBase;
    Vector3Int tilePos;

    private Tilemap parrentTilemapRef;
    public tileAlgorithmStatus cellStatus;


    public Sprite OriginalSprite { get => originalSprite; }
    public TileBase OriginalBaseTile { get => originalBaseTile; }
    public bool isOccupiedByWall { get; set; }
    public bool isOccupiedByCannonField { get; set; }
    //public bool IsOccupied;
    public Vector3Int TilePos { get => tilePos;  }
    //public tileAlgorithmStatus cellStatus { get => cellStatus; set => cellStatus = value; }

    public rampartTile(Vector2Int _tilePos2d,ref Tilemap _parrentTilemap, bool _isPlayable, bool _isCastle)
    {
        this.tilePos = convertVector2IntToVector3Int(_tilePos2d);
        parrentTilemapRef = _parrentTilemap;
        if (_parrentTilemap.GetTile(tilePos) != null) {
            isPlayable = _isPlayable;
            isOccupiedByWall = false;
            isOccupiedByCannonField = false;
            isCastle = _isCastle;
            this.originalSprite = _parrentTilemap.GetSprite(tilePos);
            originalBaseTile = _parrentTilemap.GetTile(tilePos);
        }
    }

    public void setNewTileBaseForWall(TileBase _incomingTileBase)
    {
        modifiedTileBase = _incomingTileBase;
        isOccupiedByWall = true;
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
