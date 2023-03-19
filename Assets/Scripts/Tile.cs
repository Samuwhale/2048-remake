using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public int Value { get; private set; }
    public Vector2 Position { get; private set; }

    private Image _image;
    private TextMeshProUGUI _text;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void SetColor(Color color)
    {
        _image.color = color;
    }

    public void SetText(string text)
    {
        _text.SetText(text);
    }
    
    
}