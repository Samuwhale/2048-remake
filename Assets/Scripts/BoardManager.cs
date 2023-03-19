using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    [Button("MoveTile()", ButtonSizes.Small)]
    void MoveTile()
    {
        for (int y = 0; y < _gridSize; y++)
        {
            for (int x = 0; x < _gridSize; x++)
            {
                var node = GetNodeAtGridPosition(x, y);
                if (node.HasTile())
                {
                    Debug.Log($"Node at {x}x,{y}y has value {node.CurrentTile.Value}");
                }
                else
                {
                    Debug.Log($"Node at {x}x,{y}y is empty.");
                }
            }
        }
    }
}