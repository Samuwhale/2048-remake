using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScore : MonoBehaviour
{

    public static BoardScore Instance;

    private int _score;

    public event Action<int> OnScoreChanged; 
    
    private void Start()
    {
        Tile.OnTileMerge += Tile_OnTileMerge;
        MS.Main.GameManager.OnGameReset += GameManager_OnGameReset;
    }

    private void GameManager_OnGameReset()
    {
        _score = 0;
        OnScoreChanged?.Invoke(_score);
    }

    private void Tile_OnTileMerge(int score)
    {
        _score += score;
        OnScoreChanged?.Invoke(_score);
    }

    public int GetScore()
    {
        return _score;
    }
    

    private void Awake()
    {
        if (Instance != null) return;
        {
            Instance = this;
        }
    }
}
