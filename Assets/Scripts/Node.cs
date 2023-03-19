using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public int Value { get; private set; }
    public Vector2 Position { get; private set; }
    

    public Tile CurrentTile { get; private set; }

    private void Awake()
    {
        
    }

    public void AssignTile(Tile tile)
    {
        CurrentTile = tile;
    }
    
    
}