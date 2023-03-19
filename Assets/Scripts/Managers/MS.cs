using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MS : MonoBehaviour
{
    public static MS Main { get; private set; }

    public GameManager GameManager { get; private set; }
    public UIManager UIManager { get; private set; }
    
    public EventManager EventManager { get; private set; }
    public AudioManager AudioManager { get; private set; }


    private void Start()
    {
        if (Main == null)
        {
            Main = this;
            DontDestroyOnLoad(this);
            GetSingletonComponentsInChildren();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void GetSingletonComponentsInChildren()
    {
        
        GameManager = GetComponentInChildren<GameManager>();
        UIManager = GetComponentInChildren<UIManager>();
        EventManager = GetComponentInChildren<EventManager>();
        AudioManager = GetComponentInChildren<AudioManager>();

    }
}