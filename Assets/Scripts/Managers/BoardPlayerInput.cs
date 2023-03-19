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

    private void InputReader_OnMoveEvent(Vector2 obj)
    {
        //
    }
}
