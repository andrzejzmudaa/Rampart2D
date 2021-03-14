using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//Script have to be attached to Camera
public class drawGridForTileMapUsingGL : MonoBehaviour
{

    public Material lineMat;
    public Tilemap thisTilemap;
    private Vector3 cellSize;
    private Bounds tilemapBounds;

    void DrawConnectingLines()
    {
        if (thisTilemap == null)
            return;
        this.cellSize = thisTilemap.cellSize;
        this.tilemapBounds = thisTilemap.localBounds;
        GL.Begin(GL.LINES);
        lineMat.SetPass(0);
        GL.Color(new Color(lineMat.color.r, lineMat.color.g, lineMat.color.b, lineMat.color.a));

        for (int x = (int)tilemapBounds.min.x; x < (int)tilemapBounds.max.x; x++)
        {
            GL.Vertex3(x, tilemapBounds.min.y, -1);
            GL.Vertex3(x, tilemapBounds.max.y, -1);

        }

        for (int y = (int)tilemapBounds.min.y; y < (int)tilemapBounds.max.y; y++)
        {
            GL.Vertex3(tilemapBounds.min.x , y, -1);
            GL.Vertex3(tilemapBounds.max.x , y, -1);

        }
        GL.End();

    }

    // To show the lines in the game window whne it is running
    void OnPostRender()
    {
        DrawConnectingLines();
    }

    // To show the lines in the editor
    void OnDrawGizmos()
    {
        //DrawConnectingLines();
    }


}
