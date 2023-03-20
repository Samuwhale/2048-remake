using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BoardStateManager : MonoBehaviour
{
    public static BoardStateManager Instance { get; private set; } 
    public Phase CurrentPhase { get; private set; }
    public event Action OnNewRoundStarted;

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
        if (CurrentPhase == Phase.EndOfTurn)
        {
            SwitchState(Phase.SpawnTile);
        }
    }

    private void BoardManager_OnGameLost()
    {
        if (CurrentPhase == Phase.EndOfTurn)
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
        if (CurrentPhase == Phase.Move)
        {
            SwitchState(Phase.EndOfTurn);
        }
    }

    private void BoardPlayerInput_OnValidInputReceived(Vector2 direction)
    {
        if (CurrentPhase == Phase.Input)
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



    public void SwitchState(Phase newPhase)
    {
        Debug.Log($"SwitchState({newPhase}) was called. Currently in {CurrentPhase}");
        if (CurrentPhase != newPhase)
        {
            Debug.Log($"Switched to ({newPhase})");
            CurrentPhase = newPhase;
            EnterState(newPhase);
        }
        
    }

    public void EnterState(Phase newPhase)
    {
        Debug.Log($"Entering ({newPhase})");
        switch (newPhase)
        {
            case Phase.SpawnTile:
                Debug.Log($"OnNewRoundStarted invoked");
                OnNewRoundStarted?.Invoke();
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