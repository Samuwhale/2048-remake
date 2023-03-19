using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    
    [SerializeField] private float _tileSpacing;
    

    [SerializeField] private GameObject _tilePrefab;

    private List<Tile> _tiles = new List<Tile>();
    
    
    void Start()
    {
        GenerateBoard(_width, _height);
    }
    

    void GenerateBoard(int width, int height)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var tileGameObject = Instantiate(_tilePrefab, transform);
                RectTransform tileRect = tileGameObject.GetComponent<RectTransform>();
                tileRect.anchoredPosition = new Vector2(x * 70, y * 70);

            }
        }
    }
    
    
}
