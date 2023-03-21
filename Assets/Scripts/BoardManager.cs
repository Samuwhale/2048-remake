using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    [SerializeField] private int _gridSize;

    [SerializeField] private float _tileSize;
    [SerializeField] private float _tileSpacing;


    [SerializeField] private Transform _nodePrefab;

    private List<Node> _nodes = new List<Node>();
    private List<Tile> _tiles = new List<Tile>();

    public event Action OnBoardReset;
    
    public event Action OnMoveComplete;
    public event Action OnGameWon;
    public event Action OnGameLost;

    public event Action OnReadyForNextTurn;

    private Vector3 _viewportCenter;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    void Start()
    {
        MS.Main.GameManager.OnGameReset += GameManager_ResetGame;
        _viewportCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        GenerateBoard(_gridSize);
        SpawnTile();
    }
    
    private void GameManager_ResetGame()
    {
        foreach (var node in _nodes)
        {
            node.ResetNode();
        }
        
        _tiles.Clear();
        OnBoardReset?.Invoke();
    }


    void GenerateBoard(int gridSize)
    {
        float _tileOffset = _tileSize + _tileSpacing;
        float _startPositionOffset = 0.5f * (_tileSize * _gridSize + _tileSpacing * gridSize - 1);

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                var _nodeTransform = Instantiate(_nodePrefab, transform);
                _nodeTransform.position = new Vector3(_viewportCenter.x - _startPositionOffset + _tileOffset * x,
                    _viewportCenter.y - _startPositionOffset + _tileOffset * y, 0);
                var node = _nodeTransform.GetComponent<Node>();
                node.SetGridPosition(x, y);
                _nodes.Add(node);
            }
        }
    }

    [Button("SpawnTile()", ButtonSizes.Small)]
    public void SpawnTile()
    {
        bool gridHasSpace = _nodes.Any(node => !node.HasTile());


        if (!gridHasSpace)
        {
            Debug.Log("No space in board");
            return;
        }

        List<Node> emptyNodes = _nodes.Where(node => !node.HasTile()).ToList();
        int randomIndex = Random.Range(0, emptyNodes.Count);
        var node = emptyNodes[randomIndex];
        var tile = node.SpawnTile();
        _tiles.Add(tile);
    }


    private Node GetNodeAtGridPosition(int x, int y)
    {
        return _nodes.FirstOrDefault(node => node.GridPosition == new Vector2(x, y));
    }


    public void MoveTiles(Vector2 direction)
    {
        int startX, startY;
        int endX, endY;
        int stepY, stepX;


        if (direction.x > 0)
        {
            startX = _gridSize - 1;
            endX = -1;
            stepX = -1;
        }
        else
        {
            startX = 0;
            endX = _gridSize;
            stepX = 1;
        }

        if (direction.y > 0)
        {
            startY = _gridSize - 1;
            endY = -1;
            stepY = -1;
        }
        else
        {
            startY = 0;
            endY = _gridSize;
            stepY = 1;
        }


        for (int y = startY; y != endY; y = y + stepY)
        {
            for (int x = startX; x != endX; x = x + stepX)
            {
                var node = GetNodeAtGridPosition(x, y);
                if (node.HasTile())
                {
                    TryMove(node.GetTile(), direction, node.GridPosition);
                }
            }
        }

        OnMoveComplete?.Invoke();
    }


    bool TryMove(Tile tile, Vector2 direction, Vector2 position)
    {
        int endY = direction.y > 0 ? _gridSize : -1;
        int stepY = (int)direction.y;
        int startY = (int)position.y + stepY;
        int y = startY;

        int endX = direction.x > 0 ? _gridSize : -1;
        int stepX = (int)direction.x;
        int startX = (int)position.x + stepX;
        int x = startX;

        Node targetNode = tile.OccupiedNode;
        

        while (x != endX && y != endY)
        {
            Node potentialNode = GetNodeAtGridPosition(x, y);

            Debug.Log($"potentialNode.GetNodeWasMerged() : {(potentialNode.GetNodeWasMergedThisTurn())}");
            Debug.Log($"potentialNode.HasTile() : {(potentialNode.HasTile())}");
            if (potentialNode == null) break;

            if (potentialNode.GetNodeWasMergedThisTurn()) break;

            if (potentialNode.HasTile())
            {
                if (potentialNode.GetTile().Value == tile.Value)
                {
                    Merge(tile, potentialNode);
                    return false;
                }

                break;
            }

            targetNode = potentialNode;
            x += stepX;
            y += stepY;
        }

        MoveTile(tile, targetNode);
        return true;
    }

    private void Merge(Tile tile, Node potentialNode)
    {
        potentialNode.SetNodeAsMerged();
        var tileToMerge = potentialNode.GetTile();
        tile.MergeWith(tileToMerge);
        _tiles.Remove(tile);
    }

    void MoveTile(Tile tile, Node targetNode)
    {
        // Debug.Log($"{tile.Value} at {tile.OccupiedNode.GridPosition} moved to {targetNode.GridPosition}");
        if (tile == targetNode.GetTile()) return;

        tile.OccupiedNode.ClearTile();
        targetNode.SetTile(tile);
    }


    public void ShouldGameEnd()
    {
        if (_tiles.Any(tile => tile.Value == 2048))
        {
            Debug.Log("GAME ENDED: WON");
            OnGameWon?.Invoke();
            return;
        }

        if (_nodes.Any(node => !node.HasTile()) == false)
        {
            Debug.Log("GAME ENDED: LOST");
            OnGameLost?.Invoke();
            return;
        }

        OnReadyForNextTurn?.Invoke();
    }
}