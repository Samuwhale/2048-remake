using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    [SerializeField] private float _tileAnimateSpeed = 0.2f;
    
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

    void DoubleValue()
    {
        if (Value < 2048)
        {
            Value *= 2;
            UpdateVisuals();
        }
        else
        {
            Debug.Log($"Tile already full");
        }
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


    public void DeleteSelf()
    {
        OccupiedNode.ClearTile();
        Destroy(gameObject);
    }

    public void MergeWith(Tile tileToMergeInto)
    {
        tileToMergeInto.DoubleValue();
        MoveTo(tileToMergeInto.transform.position, DeleteSelf);
    }

    
    public void MoveTo(Vector3 targetPosition, TweenCallback onComplete = null)
    {
        transform.DOMove(targetPosition, _tileAnimateSpeed).OnComplete(onComplete);
    }
}