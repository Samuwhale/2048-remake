using System;
using System.Collections;
using System.Collections.Generic;
using Sambono;
using UnityEngine;

public class BoardPlayerInput : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    private void Start()
    {
        _inputReader.MoveEvent += InputReader_OnMoveEvent;
    }

    private void InputReader_OnMoveEvent(Vector2 direction)
    {
        if (direction == Vector2.up || direction == Vector2.down || direction == Vector2.right ||
            direction == Vector2.left) { BoardManager.Instance.MoveTiles(direction);}
    }
}