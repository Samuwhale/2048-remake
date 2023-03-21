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
    [SerializeField] private float _tileAnimateSpeed = 0.15f;

    public TileVisual TileVisual { get; private set; }

    public Node OccupiedNode { get; private set; }


    [SerializeField] private SpriteRenderer _spriteRenderer;

    [SerializeField] private TextMeshPro _text;

    public static event Action<int> OnTileMerge; 

    private void Awake()
    {
        TileVisual = GetComponent<TileVisual>();
    }

    private void Start()
    {
        InitializeValue();
        TileVisual.PlaySpawnAnimation();
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
            OnTileMerge?.Invoke(Value);
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
        TileVisual.PlayDeleteAnimation();
        OccupiedNode.ClearTile();
        Destroy(gameObject, 0.3f);
    }

    public void MergeWith(Tile tileToMergeInto)
    {
        tileToMergeInto.DoubleValue();
        tileToMergeInto.TileVisual.PlayMergeAnimation();
        DeleteSelf();
        MoveTo(tileToMergeInto.transform.position);
    }
    
    public void MoveTo(Vector3 targetPosition)
    {
        transform.DOMove(targetPosition, _tileAnimateSpeed);
    }
}