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
    [SerializeField] private ValueColorsSO _valueColorsSo;


    

    void Start()
    {
        BoardScore.Instance.OnScoreChanged += BoardScore_OnScoreChanged;
        UpdateScore();
    }
    

    private void BoardScore_OnScoreChanged(int score)
    {
        
        _scoreValueText.DOColor(_valueColorsSo.TileValueColors[Random.Range(0, _valueColorsSo.TileValueColors.Length)].Color, 0.5f);
        _scoreValueText.SetText(score.ToString());
    }

    private void UpdateScore()
    {
        _scoreValueText.SetText(BoardScore.Instance.GetScore().ToString());
    }


}
