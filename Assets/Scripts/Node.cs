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

    private bool _tileWaMergedThisTurn;

    public Tile _currentTile;

    private void Start()
    {
        BoardStateManager.Instance.OnNewRoundStarted += BoardStateManager_OnNewRoundStarted;
    }

    private void BoardStateManager_OnNewRoundStarted()
    {
        Debug.Log($"BoardStateManager_OnNewRoundStarted() {GridPosition} tileMerged {_tileWaMergedThisTurn} -> false");
        _tileWaMergedThisTurn = false;
    }

    public void ResetNode()
    {
        if (HasTile()) GetTile().DeleteSelf();
        _tileWaMergedThisTurn = false;
    }
    
    public bool GetNodeWasMergedThisTurn()
    {
        Debug.Log($"GetNodeWasMergedThisTurn() {GridPosition} tileMerged {_tileWaMergedThisTurn}");
        return _tileWaMergedThisTurn;
    }

    public void SetNodeAsMerged()
    {
        Debug.Log($"SetNodeAsMerged() {GridPosition} tileMerged {_tileWaMergedThisTurn} -> true");
        _tileWaMergedThisTurn = true;
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
        tile.MoveTo(transform.position);
        
        tile.transform.SetParent(transform);
        
        
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
        // if (HasTile()) GetTile().DeleteSelf();
        _currentTile = null;
    }
}