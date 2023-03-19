using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardStateManager : MonoBehaviour
{
    public BoardStateManager Instance { get; private set; }

    public enum State
    {
        WaitingForInput,
        Move,
        CheckGameOver,
    }

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    public State CurrentState;

    public void SwitchState(State newState)
    {
        if (CurrentState != newState)
        {
            CurrentState = newState;
        }
    }
}