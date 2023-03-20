using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardStateManager : MonoBehaviour
{
    public BoardStateManager Instance { get; private set; }

    public enum Phase
    {
        SpawnTile,
        Input,
        Move,
        EndOfTurn,
        GameOver,
    }

    private void Start()
    {
        BoardPlayerInput.Instance.OnValidInputReceived += BoardPlayerInput_OnValidInputReceived;
        BoardManager.Instance.OnMoveComplete += BoardManager_OnMoveComplete;
        BoardManager.Instance.OnReadyForNextTurn += BoardManager_OnReadyForNextTurn;
        BoardManager.Instance.OnGameLost += BoardManager_OnGameLost;
        BoardManager.Instance.OnGameWon += BoardManager_OnGameWon;
        
        SwitchState(Phase.Input);
    }

    private void BoardManager_OnReadyForNextTurn()
    {
        if (currentPhase == Phase.EndOfTurn)
        {
            SwitchState(Phase.SpawnTile);
        }
    }

    private void BoardManager_OnGameLost()
    {
        if (currentPhase == Phase.EndOfTurn)
        {
            SwitchState(Phase.GameOver);
        }
    }

    private void BoardManager_OnGameWon()
    {
        // if (currentPhase == Phase.EndOfTurn)
        // {
                // WIN!!! 
        // }
        throw new NotImplementedException();
    }

    private void BoardManager_OnMoveComplete()
    {
        if (currentPhase == Phase.Move)
        {
            SwitchState(Phase.EndOfTurn);
        }
    }

    private void BoardPlayerInput_OnValidInputReceived(Vector2 direction)
    {
        if (currentPhase == Phase.Input)
        {
            SwitchState(Phase.Move);
            BoardManager.Instance.MoveTiles(direction);
        }
    }

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    public Phase currentPhase;

    public void SwitchState(Phase newPhase)
    {
        if (currentPhase != newPhase)
        {
            currentPhase = newPhase;
            EnterState(newPhase);
        }
    }

    public void EnterState(Phase newPhase)
    {
        switch (newPhase)
        {
            case Phase.SpawnTile:
                BoardManager.Instance.SpawnTile();
                SwitchState(Phase.Input);
                break;
            case Phase.Input:
                break;
            case Phase.Move:
                break;
            case Phase.EndOfTurn:
                BoardManager.Instance.ShouldGameEnd();
                break;
            case Phase.GameOver:
                Debug.Log("YOU LOST");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newPhase), newPhase, null);
        }
    }
}