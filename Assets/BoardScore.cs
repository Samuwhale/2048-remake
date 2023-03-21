using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScore : MonoBehaviour
{

    public static BoardScore Instance;

    private int _score = 0;
    private int _hiScore = 0;

    public event Action<int> OnScoreChanged; 
    public event Action<int> OnHighScoreChanged; 
    
    
    private void Start()
    {
        Tile.OnTileMerge += Tile_OnTileMerge;
        MS.Main.GameManager.OnGameReset += GameManager_OnGameReset;
    }

    private void GameManager_OnGameReset()
    {
        SetScore(0);
    }

    
    
    
    private void Tile_OnTileMerge(int score)
    {
        SetScore(_score + score);
        
    }

    public int GetScore()
    {
        return _score;
    }

    public int GetHighScore()
    {
        return _hiScore;
    }

    void SetScore(int score)
    {
        _score = score;
        if (_score > _hiScore)
        {
            _hiScore = score;
            PlayerPrefs.SetInt("HighScore", _hiScore);
            PlayerPrefs.Save();
            OnHighScoreChanged?.Invoke(_hiScore);
        }
        
        OnScoreChanged?.Invoke(_score);
    }

    private void Awake()
    {
        if (Instance != null) return;
        {
            Instance = this;
        }

        _hiScore = PlayerPrefs.GetInt("HighScore", 0);
    }
}
