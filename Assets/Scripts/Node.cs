using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public Vector2 GridPosition { get; private set; }

    [SerializeField] private Transform _tilePrefab;
    

    public Tile _currentTile;

    public void AssignTile(Tile tile)
    {
        _currentTile = tile;
    }

    public bool HasTile()
    {
        return _currentTile != null;
    }

    public Tile GetTile()
    {
        return _currentTile;
    }
    
    public void SetGridPosition(int x, int y)
    {
        GridPosition = new Vector2(x, y);
    }

    public void SetTile(Tile tile)
    {
        if (tile == _currentTile) return;
        tile.transform.SetParent(transform);
        tile.MoveTo(transform.position);
        _currentTile = tile;
        tile.SetNode(this);
    }

    public Tile SpawnTile()
    {
        var tileTransform = Instantiate(_tilePrefab, transform.position, Quaternion.identity);
        Tile tile = tileTransform.GetComponent<Tile>();
        SetTile(tile);
        return tile;
    }

    public void ClearTile()
    {
        _currentTile = null;
    }
}