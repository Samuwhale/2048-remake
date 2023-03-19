using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public Vector2 GridPosition { get; private set; }

    [SerializeField] private Transform _tilePrefab;

    public Tile CurrentTile { get; private set; }

    public void AssignTile(Tile tile)
    {
        CurrentTile = tile;
    }

    public bool HasTile()
    {
        return CurrentTile != null;
    }
    
    public void SetGridPosition(int x, int y)
    {
        GridPosition = new Vector2(x, y);
    }

    public void SetTile(Tile tile)
    {
        tile.transform.SetParent(transform);
        tile.transform.localPosition = Vector3.zero;
        CurrentTile = tile;
        tile.SetNode(this);
    }

    public Tile SpawnTile()
    {
        var tileTransform = Instantiate(_tilePrefab);
        Tile tile = tileTransform.GetComponent<Tile>();
        SetTile(tile);
        return tile;
    }
}