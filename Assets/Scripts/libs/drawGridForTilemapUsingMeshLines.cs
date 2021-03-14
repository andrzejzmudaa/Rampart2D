using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class drawGridForTilemapUsingMeshLines : MonoBehaviour
{
    // Start is called before the first frame update

    public Tilemap thisTilemap;
    public float lineSize;
    private Color linesColor;
    private Vector3 cellSize;
    private Bounds tilemapBounds;
    private GameObject meshObject;
    private Dictionary<int, Vector3> meshLinesVertices;
    private List<Color> meshLinesColors;




    void Start()
    {
        initValues();

        Mesh mesh = new Mesh();

        int meshKey = 0;

        for (float x = tilemapBounds.min.x; x < tilemapBounds.max.x; x += cellSize.x)
        {
            meshLinesVertices.Add(meshKey++, new Vector3(x, tilemapBounds.min.y, 0));
            meshLinesVertices.Add(meshKey++, new Vector3(x, tilemapBounds.max.y, 0));
            meshLinesColors.Add(linesColor);

        }

        for (float y = tilemapBounds.min.y; y < tilemapBounds.max.y; y += cellSize.y)
        {
            meshLinesVertices.Add(meshKey++, new Vector3(tilemapBounds.min.x, y, 0));
            meshLinesVertices.Add(meshKey++, new Vector3(tilemapBounds.max.x, y, 0));
            meshLinesColors.Add(linesColor);

        }



        mesh.colors = meshLinesColors.ToArray();
        mesh.vertices = (new List<Vector3>(meshLinesVertices.Values)).ToArray();


        int[] indiceArray = new int[meshLinesVertices.Count];
        meshLinesVertices.Keys.CopyTo(indiceArray, 0);
        mesh.SetIndices(indiceArray, MeshTopology.Lines, 0, true);

        meshObject.GetComponent<MeshFilter>().sharedMesh = mesh;


        Debug.Log("Mesh Vertices : " + meshLinesVertices);
        Debug.Log("Bounds x size : " + tilemapBounds.size.x);
        Debug.Log("Bounds x min: " + tilemapBounds.min.x);
        Debug.Log("Bounds x max: " + tilemapBounds.max.x);
        Debug.Log("Cellsize: " + cellSize.x);

    }

    private void initValues()
    {
        if (thisTilemap == null)
            return;
        meshLinesVertices = new Dictionary<int, Vector3>();
        meshLinesColors = new List<Color>();
        linesColor = Color.black;
        this.cellSize = thisTilemap.cellSize;
        this.tilemapBounds = thisTilemap.localBounds;
        meshObject = new GameObject("meshLineRenderer", typeof(MeshRenderer), typeof(MeshFilter));
        meshObject.transform.SetParent(transform);
        meshObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 1);
        meshObject.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Specular"));

    }
}
