using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreValueText;
    [SerializeField] private TextMeshProUGUI _highScoreValueText;
    [SerializeField] private ValueColorsSO _valueColorsSo;


    void Start()
    {
        BoardScore.Instance.OnScoreChanged += BoardScore_OnScoreChanged;
        BoardScore.Instance.OnHighScoreChanged += BoardScore_OnHighScoreChanged;
        UpdateScoreText();
        UpdateHighScoreText();
    }

    private void BoardScore_OnHighScoreChanged(int obj)
    {
        ChangeHighScoreColors();
        UpdateHighScoreText();
    }


    private void BoardScore_OnScoreChanged(int score)
    {
        ChangeScoreColors();
        UpdateScoreText();
    }

    void ChangeScoreColors()
    {
        if (_scoreValueText != null)
        {
            _scoreValueText.DOColor(
                _valueColorsSo.TileValueColors[Random.Range(0, _valueColorsSo.TileValueColors.Length)].Color, 0.5f);
        }
    }


    private void UpdateScoreText()
    {
        if (_scoreValueText != null)
        {
            _scoreValueText.SetText(BoardScore.Instance.GetScore().ToString());
        }
        
    }
    
    void ChangeHighScoreColors()
    {


        if (_highScoreValueText != null)
        {
            _highScoreValueText.DOColor(
                _valueColorsSo.TileValueColors[Random.Range(0, _valueColorsSo.TileValueColors.Length)].Color, 0.5f);
        }
    }


    private void UpdateHighScoreText()
    {
   

        if (_highScoreValueText != null)
        {
            _highScoreValueText.SetText(BoardScore.Instance.GetHighScore().ToString());
        }
    }
}