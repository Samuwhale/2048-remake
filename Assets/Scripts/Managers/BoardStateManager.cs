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
    public event Action OnBoardGameOver;

    public enum Phase
    {
        ResettingBoard,
        SpawnTile,
        Input,
        Move,
        EndOfTurn,
        EndOfGame,
    }

    private void Start()
    {
        BoardPlayerInput.Instance.OnValidInputReceived += BoardPlayerInput_OnValidInputReceived;
        BoardManager.Instance.OnMoveComplete += BoardManager_OnMoveComplete;
        BoardManager.Instance.OnReadyForNextTurn += BoardManager_OnReadyForNextTurn;
        BoardManager.Instance.OnGameLost += BoardManager_OnGameLost;
        BoardManager.Instance.OnGameWon += BoardManager_OnGameWon;
        BoardManager.Instance.OnBoardReset += BoardManager_OnBoardReset;
        MS.Main.GameManager.OnGameReset += GameManager_ResetGame;
        

        SwitchState(Phase.Input);
    }

    private void BoardManager_OnBoardReset()
    {
        if (CurrentPhase == Phase.ResettingBoard)
        {
            SwitchState(Phase.SpawnTile);
        }
    }

    private void GameManager_ResetGame()
    {
        SwitchState(Phase.ResettingBoard);
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
        SwitchState(Phase.EndOfGame);
    }

    private void BoardManager_OnGameWon()
    {
        SwitchState(Phase.EndOfGame);
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
            case Phase.ResettingBoard:
                break;
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
            case Phase.EndOfGame:
                
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newPhase), newPhase, null);
        }
    }
}