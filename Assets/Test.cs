using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject test1;

    void Start()
    {
        Tilemap thisTilemap = GetComponent<Tilemap>();
        if (thisTilemap == null)
            return;
        for (int x = 0; x < 10; x++) {
            test1 = new GameObject();
            test1.AddComponent<LineRenderer>();

        }
        
    }


}
