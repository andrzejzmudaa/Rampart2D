using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class countFieldScript : MonoBehaviour
{
    Tilemap playerFieldMap;
    int countTiles;
    // Start is called before the first frame update
    void Start()
    {
        playerFieldMap = GetComponentInChildren<Tilemap>();
        if (playerFieldMap == null)
            return;
        drawSameMapWithTileBorders(playerFieldMap);



    }


    GameObject drawSameMapWithTileBorders(Tilemap inputMap)
    {
        GameObject bordersMapObject = Instantiate(inputMap.gameObject);
        bordersMapObject.transform.SetParent(inputMap.transform.parent);
        bordersMapObject.transform.position.Set(inputMap.transform.position.x, inputMap.transform.position.y, inputMap.transform.position.z -1);
        Tilemap bordersMap = bordersMapObject.GetComponent<Tilemap>();
        for (int x = 0; x < bordersMap.size.x; x++)
        {
            for (int y = 0; y < bordersMap.size.y; y++)
            {
                Vector3Int tempVector = new Vector3Int(x, y, 0);
                if (bordersMap.GetSprite(tempVector) != null)
                    bordersMap.SetColor(tempVector, Color.red);
                Tile localTile = (Tile)bordersMap.GetTile(tempVector);
                //localTile.color = Color.yellow;


            }
        }




        return bordersMapObject;

    }
}
