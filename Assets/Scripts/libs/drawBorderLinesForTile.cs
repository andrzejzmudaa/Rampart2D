using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class drawBorderLinesForTile : MonoBehaviour
{
    public Material lineRendererMaterial;
    private Tilemap thisTilemap;
    private Vector3 cellSize;
    private Bounds tilemapBounds;
    private GameObject linesContainer;
    private LineRenderer linesRendererPatern;
    private int createdLinesNumber;
    private int linesContainerCounter;
    int loopCycles;

    void Start()
    {
        thisTilemap = GetComponent<Tilemap>();
        if (thisTilemap == null)
            return;
        createLinesRendererPatern();

        this.cellSize = thisTilemap.cellSize;
        this.tilemapBounds = thisTilemap.localBounds;

        for (int x = (int)tilemapBounds.min.x; x < (int)tilemapBounds.max.x; x++)
        {
            loopCycles++;
            createLineContainer();
            createdLinesNumber++;
            LineRenderer locallineRenderer = Instantiate(linesRendererPatern, linesContainer.transform);
            locallineRenderer.name = "Line " + createdLinesNumber;
            locallineRenderer.GetComponent<LineRenderer>().SetPositions(new[] { new Vector3(x, tilemapBounds.min.y, -2f), new Vector3(x, tilemapBounds.max.y, -2f) });


        }


        Debug.Log("Rozmiar mapy min x " + tilemapBounds.min.x);
        Debug.Log("Rozmiar mapy max x " + tilemapBounds.max.x);
        Debug.Log("Loop cycles " + loopCycles);



        Destroy(linesRendererPatern);
    }

    private void createLinesRendererPatern()
    {
        float lineWidth = 0.1f;
        linesRendererPatern = new GameObject().AddComponent<LineRenderer>();
        linesRendererPatern.name = "Line Renderer Pattern";
        linesRendererPatern.startWidth = lineWidth;
        linesRendererPatern.endWidth = lineWidth;
        //linesRendererPatern.startColor = Color.black;
        //linesRendererPatern.endColor = Color.black;
        //linesRendererPatern.colorGradient = getSolidColorGradient(Color.black);
        //linesRendererPatern.material.color = Color.black;
        linesRendererPatern.material = lineRendererMaterial;
    }

    private void createLineContainer()
    {
        int gameObjectMaxChilldrensAmount = 25;
        if (createdLinesNumber % gameObjectMaxChilldrensAmount == 0)
        {
            linesContainer = new GameObject();
            linesContainer.transform.SetParent(thisTilemap.transform);
            linesContainer.name = "Lines Container " + linesContainerCounter;
            linesContainerCounter++;
        }
    }

    private Gradient getSolidColorGradient(Color colorWanted)
    {
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = colorWanted;
        colorKey[0].time = 0.0f;
        colorKey[1].color = colorWanted;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 0.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        return gradient;
    }
}
