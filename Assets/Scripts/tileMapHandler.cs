using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class tileMapHandler : MonoBehaviour
{
    Tilemap thisTilemap;
    TilemapCollider2D thisTileMapCollider2D;
    int tileMapSizeX;
    int tileMapCellAmountY;
    int tileMapSingleCellSizeX;
    int tileMapSingleCellSizeY;
    GameObject parentObject;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;


    // Start is called before the first frame update
    void Start()
    {
        thisTilemap = this.GetComponent<Tilemap>();
        thisTileMapCollider2D = this.GetComponent<TilemapCollider2D>();
        //Mesh mesh = thisTileMapCollider2D.CreateMesh(true,true);
        parentObject = this.gameObject.transform.parent.gameObject;
        meshFilter = parentObject.AddComponent<MeshFilter>();
        meshRenderer = parentObject.AddComponent<MeshRenderer>();
        //meshFilter.mesh = mesh;

        //tileMapSizeX = thisTilemap.size.x;
        //tileMapCellAmountY = thisTilemap.size.y;
        //tileMapSingleCellSizeX = thisTilemap.cellSize.x;
        //tileMapSingleCellSizeY = thisTilemap.cellSize.y;

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector2 mousePos = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector3Int gridPosition = thisTilemap.WorldToCell(mousePos);

        //    print("Grid "+ thisTilemap.GetTile(gridPosition).name);
        //    print("test " + thisTilemap.GetCellCenterWorld(gridPosition));



        //}
        // Debug.Log("thisTileMapCollider2D Bounds: " + thisTileMapCollider2D.bounds);
        //Debug.Log("Map size : " + tilemap.size);
        //Debug.Log("Map size : " + tilemap.size);
        //Debug.Log("Map size : " + tilemap.size);



        //bool[,] array2D = { { false, false, false, false },
        //                    { true, false, false, false },
        //                    { true, true,false,false },
        //                    { true, true,true,false }};

        //bool[] array0 = { false, false,false,false };
        //bool[] array1 = { true, false, false, false };
        //bool[] array2 = { true, true, false, false };
        //bool[] array3 = { true, true, true, false };

        //brickProperties.convert1DsArraysToSingle2D(array0, array1, array2, array3);


        //for (int row = 0; row < 4; row++)
        //{
        //    for (int collumn = 0; collumn < 4; collumn++)
        //    {
        //        Debug.Log("Element [" + row + "][" + collumn + "] = " + array2D[row, collumn]);
        //    }
        //}

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }


}
