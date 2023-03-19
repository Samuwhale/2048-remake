using System;
using System.Collections;
using System.Collections.Generic;
using Sambono;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public SettingsUI SettingsUI {get; private set;}

    public PauseScreenUI PauseScreenUI {get; private set;}

    public GameOverUI GameOverUI {get; private set;}

    private void Start()
    {
        GetScriptsFromChildren();
    }


    void GetScriptsFromChildren()
    {
        SettingsUI = GetComponentInChildren<SettingsUI>();
        
        
        PauseScreenUI = GetComponentInChildren<PauseScreenUI>();
        
        
        GameOverUI = GetComponentInChildren<GameOverUI>();
        
    }
}