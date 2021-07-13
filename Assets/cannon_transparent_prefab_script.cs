using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class cannon_transparent_prefab_script : MonoBehaviour
{
    Text cannonAmmountToPutText;
    // Start is called before the first frame update
    void Start()
    {
        cannonAmmountToPutText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void executeAssigingInternalTextReference()
    {
        cannonAmmountToPutText = this.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
    }

    public void updatecannonAmmountToPutText(int _numberToUpdate)
    {
        cannonAmmountToPutText.text = _numberToUpdate.ToString();
    }

    public void setInitialPositionInMiddle(Tilemap _playerMap)
    {
        this.gameObject.transform.position = new Vector3(Mathf.RoundToInt((_playerMap.localBounds.max.x - (_playerMap.localBounds.max.x - _playerMap.localBounds.min.x) / 2) + _playerMap.transform.position.x), Mathf.RoundToInt((_playerMap.localBounds.max.y - (_playerMap.localBounds.max.y - _playerMap.localBounds.min.y) / 2) + _playerMap.transform.position.y), this.gameObject.transform.position.z);
    }

    public bool moveCannonUp(Tilemap _playerMap)
    {
        return checkIfCannonMovementPossible(this.gameObject.transform.position + Vector3.up, _playerMap);
    }

    public bool moveCannonDown(Tilemap _playerMap)
    {
        return checkIfCannonMovementPossible(this.gameObject.transform.position + Vector3.down, _playerMap);
    }

    public bool moveCannonLeft(Tilemap _playerMap)
    {
        return checkIfCannonMovementPossible(this.gameObject.transform.position + Vector3.left, _playerMap);
    }

    public bool moveCannonRight(Tilemap _playerMap)
    {
        return checkIfCannonMovementPossible(this.gameObject.transform.position + Vector3.right, _playerMap);
    }

    public bool checkIfCannonMovementPossible(Vector3 _movement, Tilemap _playerMap)
    {
        bool isMovementPossible = false;
        if (_playerMap.localBounds.max.x >= _movement.x + 1 && _playerMap.localBounds.min.x <= _movement.x - 1 && _playerMap.localBounds.max.y >= _movement.y + 1 && _playerMap.localBounds.min.y <= _movement.y - 1)
        {
            this.gameObject.transform.position = _movement;
            isMovementPossible = true;
        }
        return isMovementPossible;
    }

    public bool checkIfPutCannonOnPlacetPossibleAndSet(Tilemap _CannonMap, Dictionary<Vector2Int, rampartTile> _playerMap , PlayerManager _callingPlayerManager)
    {
        Vector2Int cannonPlace = new Vector2Int((int)this.gameObject.transform.position.x, (int)this.gameObject.transform.position.y);
        rampartTile tempRampartTile;
        List<rampartTile> tempRampartTileListToModify = new List<rampartTile>();
        bool isPutCannonOnPlacePossible = true;
        if (_playerMap.TryGetValue(cannonPlace + new Vector2Int(-1, 0), out tempRampartTile))
        {
            if (tempRampartTile.isPlayable && !tempRampartTile.isOccupiedByWall && !tempRampartTile.isOccupiedByCannonField)
                tempRampartTileListToModify.Add(tempRampartTile);
            else
                isPutCannonOnPlacePossible = false;
        }
        else
            isPutCannonOnPlacePossible = false;

        if (_playerMap.TryGetValue(cannonPlace + new Vector2Int(-1, -1), out tempRampartTile))
        {
            if (tempRampartTile.isPlayable && !tempRampartTile.isOccupiedByWall && !tempRampartTile.isOccupiedByCannonField)
                tempRampartTileListToModify.Add(tempRampartTile);
            else
                isPutCannonOnPlacePossible = false;
        }
        else
            isPutCannonOnPlacePossible = false;

        if (_playerMap.TryGetValue(cannonPlace + new Vector2Int(0, -1), out tempRampartTile))
        {
            if (tempRampartTile.isPlayable && !tempRampartTile.isOccupiedByWall && !tempRampartTile.isOccupiedByCannonField && tempRampartTile.cellStatus == rampartTile.tileAlgorithmStatus.Internal)
                tempRampartTileListToModify.Add(tempRampartTile);
            else
                isPutCannonOnPlacePossible = false;
        }
        else
            isPutCannonOnPlacePossible = false;

        if (_playerMap.TryGetValue(cannonPlace + new Vector2Int(0, 0), out tempRampartTile))
        {
            if (tempRampartTile.isPlayable && !tempRampartTile.isOccupiedByWall && !tempRampartTile.isOccupiedByCannonField)
                tempRampartTileListToModify.Add(tempRampartTile);
            else
                isPutCannonOnPlacePossible = false;
        }
        else
            isPutCannonOnPlacePossible = false;

        if (isPutCannonOnPlacePossible)
        {
            cannon_prefab_script tempCannonRef = _callingPlayerManager.instantiateCannonPrefab(cannonPlace);
            foreach (rampartTile singleTile in tempRampartTileListToModify)
            {
                singleTile.isOccupiedByCannonField = true;
                singleTile.parentCannon = tempCannonRef;
            }
        }
        
        return isPutCannonOnPlacePossible;
    }

    public void setCannonMaterialColor(Color _inputColor)
    {
        this.gameObject.GetComponent<SpriteRenderer>().material.color = _inputColor * 6f;
    }
}