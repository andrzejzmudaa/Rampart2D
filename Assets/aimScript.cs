using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class aimScript : MonoBehaviour
{
    float movementFactor = 0.05f;
    float vertExtent;
    float horzExtent;
    private UnityEngine.Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = UnityEngine.Camera.main;
        vertExtent = cam.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
        //Camera.orthographicSize
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool moveAimUp()
    {
        return checkIfAimMovementPossible(Vector2.up * movementFactor);
    }

    public bool moveAimDown()
    {
        return checkIfAimMovementPossible(Vector2.down * movementFactor);
    }

    public bool moveAimLeft()
    {
        return checkIfAimMovementPossible(Vector2.left * movementFactor);
    }

    public bool moveAimRight()
    {
        return checkIfAimMovementPossible(Vector2.right * movementFactor);
    }

    public bool checkIfAimMovementPossible(Vector2 _movement)
    {
        bool isMovementPossible = false;
        if (this.gameObject.transform.position.x + _movement.x <= horzExtent && this.gameObject.transform.position.x + _movement.x >= -horzExtent && this.gameObject.transform.position.y + _movement.y <= vertExtent && this.gameObject.transform.position.y + _movement.y >= -vertExtent)
        {
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x + _movement.x, this.gameObject.transform.position.y + _movement.y, this.gameObject.transform.position.z);
            isMovementPossible = true;
        }
        return isMovementPossible;
    }
    public void setAimColor(Color _aimColor)
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = _aimColor;
    }

    public void setAimMaterialColor(Color _inputColor)
    {
        this.gameObject.GetComponent<SpriteRenderer>().material.color = _inputColor * 6f;
    }

    public void setInitialPositionInMiddle(Tilemap _playerMap)
    {
        this.gameObject.transform.position = new Vector3(Mathf.RoundToInt((_playerMap.localBounds.max.x - (_playerMap.localBounds.max.x - _playerMap.localBounds.min.x) / 2) + _playerMap.transform.position.x), Mathf.RoundToInt((_playerMap.localBounds.max.y - (_playerMap.localBounds.max.y - _playerMap.localBounds.min.y) / 2) + _playerMap.transform.position.y), this.gameObject.transform.position.z);
    }
}
