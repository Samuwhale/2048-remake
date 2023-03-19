using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class Tile : MonoBehaviour
{
    public int Value { get; private set; }
    [SerializeField] private int[] _spawnValues;
    [SerializeField] private ValueColorsSO _valueColorsSo;
    public Node OccupiedNode { get; private set; }


    [FormerlySerializedAs("_image")] [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private TextMeshPro _text;


    private void Start()
    {
        InitializeValue();
    }

    void InitializeValue()
    {
        Value = _spawnValues[Random.Range(0, _spawnValues.Length)];
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        SetText(Value.ToString());
        SetColor(_valueColorsSo.GetColorFromValue(Value));
    }

    private void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    private void SetText(string text)
    {
        _text.SetText(text);
    }

    public void SetNode(Node node)
    {
        OccupiedNode = node;
    }

    private void MoveTile(Vector2 direction)
    {
        //
    }
}