
using UnityEngine;
using UnityEngine.Tilemaps;

public class debug_script : MonoBehaviour
{
    private UnityEngine.Camera cam;
    public Tilemap inputTileMap;
    private Tilemap wallTileMap;
    public PlayerManager playerClass;
    void Start()
    {
        cam = UnityEngine.Camera.main;
        
    }

    private void Update()
    {
        wallTileMap = inputTileMap.gameObject.transform.GetChild(0).GetComponent<Tilemap>();
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            
            Debug.Log("TIle: " + wallTileMap.GetTile(wallTileMap.WorldToCell(pos)));
            playerClass.verifyPlayerFieldHit(wallTileMap.WorldToCell(pos));

        }
    }




}