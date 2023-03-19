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

    private Vector3 _viewportCenter;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    void Start()
    {
        _viewportCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        GenerateBoard(_gridSize);
    }

    [Button("GenerateBoard()", ButtonSizes.Small)]
    void GenerateBoardDebug()
    {
        GenerateBoard(_gridSize);
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
    void SpawnTile()
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
                    TryMove(node.CurrentTile, direction, node.GridPosition);
                }
            }
        }
    }

    // these functions can PROBABLY be moved into a single one that just iterates using the direction
    void TryMove(Tile tile, Vector2 direction, Vector2 position)
    {
        if (direction.x != 0) TryMoveHorizontal(tile, direction, position);
        if (direction.y != 0) TryMoveVertical(tile, direction, position);
    }

    void TryMoveVertical(Tile tile, Vector2 direction, Vector2 position)
    {
        int end = direction.y > 0 ? _gridSize : -1;
        int step = (int)direction.y;
        int start = (int)position.y + step;

        if (0 > start || start > _gridSize - 1)
        {
            Debug.Log($"Tile is at border ({tile.OccupiedNode.GridPosition})");
            return;
        }

        Node targetNode = tile.OccupiedNode;
        for (int y = start; y != end; y += step)
        {
            Node potentialNode = GetNodeAtGridPosition((int)position.x, y);
            if (potentialNode.HasTile())
            {
                break;
            }

            targetNode = potentialNode;
        }

        MoveTile(tile, targetNode);
    }

    void TryMoveHorizontal(Tile tile, Vector2 direction, Vector2 position)
    {
        int end = direction.x > 0 ? _gridSize : -1;
        int step = (int)direction.x;
        int start = (int)position.x + step;

        if (0 > start || start > _gridSize - 1)
        {
            Debug.Log($"Tile is at border ({tile.OccupiedNode.GridPosition})");
            return;
        }

        Node targetNode = tile.OccupiedNode;
        for (int x = start; x != end; x += step)
        {
            Node potentialNode = GetNodeAtGridPosition(x, (int)position.y);
            if (potentialNode.HasTile())
            {
                break;
            }

            targetNode = potentialNode;
        }

        MoveTile(tile, targetNode);
    }

    void MoveTile(Tile tile, Node targetNode)
    {
        Debug.Log($"{tile.Value} at {tile.OccupiedNode.GridPosition} moved to {targetNode.GridPosition}");
        tile.OccupiedNode.ClearTile();
        targetNode.SetTile(tile);
    }
}