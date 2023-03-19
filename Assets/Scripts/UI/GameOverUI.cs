using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private GameObject _visibilityObject;
    [SerializeField] private Button _playAgainButton;

    private void Awake() {
        _playAgainButton.onClick.AddListener(OnPlayAgainClicked);
        _playAgainButton.Select();
    }

    private void Start()
    {
        Hide();
    }


    void OnPlayAgainClicked() {
        
    }
    
    

    public void Show()
    {
        _visibilityObject.SetActive(true);
    }
    
    public void Hide()
    {
        _visibilityObject.SetActive(false);
    }
    
}
