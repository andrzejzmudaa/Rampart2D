using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class mapFunctions_libs
{
    public static bool findAndMarkClosedShapes(Dictionary<Vector2Int, rampartTile> _input_dictionary,ref Tilemap _TileMap_ref)
    {
        Vector3Int firstBorderTileNotOccupiedVector3Int = findFirstTileNotOccupied(_input_dictionary);
        if( firstBorderTileNotOccupiedVector3Int.z == 1 )
        {
            // Another function if there is no empty in border line
        }




        return false;
    }

    static Vector3Int findFirstTileNotOccupied(Dictionary<Vector2Int, rampartTile> _input_dictionary)
    {
        Vector3Int tempVector3Int = new Vector3Int(0, 0, 1);
        foreach (rampartTile singleRampartTile in _input_dictionary.Values)
        {
            if( singleRampartTile.isPlayable && !singleRampartTile.isOccupied && singleRampartTile.borderTile)
            {
                tempVector3Int = singleRampartTile.TilePos;
                break;
            }
                    
        }
        return tempVector3Int;
    }

    static void findEachTileClosedInternally(Dictionary<Vector2Int, rampartTile> _input_dictionary, ref Tilemap _TileMap_ref)
    {
        //Dictionary < Vector2Int, rampartTile > tempDictionary = removeFromDictionaryAllTilesBasedOnBorder(_input_dictionary, _TileMap_ref.cellBounds);
        int mapMinSizeX = _TileMap_ref.cellBounds.xMin;
        int mapMaxSizeX = _TileMap_ref.cellBounds.xMax;
        int mapMinSizeY = _TileMap_ref.cellBounds.yMin;
        int mapMaxSizeY = _TileMap_ref.cellBounds.yMax;
        




    }

    static Dictionary<Vector2Int, rampartTile> setStatusForAllTilesBasedOnBorder(Dictionary<Vector2Int, rampartTile> _input_dictionary, BoundsInt _inputBounds)
    {
        IDictionaryEnumerator myEnumerator = _input_dictionary.GetEnumerator();
        while (myEnumerator.MoveNext())
        {
            Vector2Int key = (Vector2Int)myEnumerator.Key;
            if (key.x == _inputBounds.xMin || key.x == _inputBounds.xMax || key.y == _inputBounds.yMin || key.y == _inputBounds.yMax)
            {
                _input_dictionary.Remove(key);
            }
        }
        return _input_dictionary;
    }
}
