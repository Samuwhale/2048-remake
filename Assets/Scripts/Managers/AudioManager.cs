using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour {
    [SerializeField] private AudioSource _bgmSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioMixer _masterMixer;

    [SerializeField] private AudioClip _testClip;

    [SerializeField] private AudioClip[] _swishes;
    [SerializeField] private AudioClip[] _merges;
    [SerializeField] private AudioClip _2048;
    [SerializeField] private AudioClip _gameOver;


    
    
    public float MusicVolume { get; private set; }
    private const string PLAYERPREFS_MUSICVOLUME = "MusicVolume";
    
    public float MasterVolume { get; private set; }
    private const string PLAYERPREFS_MASTERVOLUME = "MasterVolume";
    
    public float SfxVolume { get; private set; }
    private const string PLAYERPREFS_SFXVOLUME = "SfxVolume";


    public void PlaySwish()
    {
        PlaySfxFromArray(_swishes);
    }
    public void PlayMerge()
    {
        PlaySfxFromArray(_merges);
    }

    public void PlayGameOver()
    {
        PlaySfx(_gameOver);
    }

    public void Play2048()
    {
        PlaySfx(_2048);
    }
    
    private void Awake()
    {
     
        MusicVolume = PlayerPrefs.GetFloat(PLAYERPREFS_MUSICVOLUME, -8);
        _masterMixer.SetFloat("MusicVolume", MusicVolume);
        
        MasterVolume = PlayerPrefs.GetFloat(PLAYERPREFS_MASTERVOLUME, -8);
        _masterMixer.SetFloat("MasterVolume", MasterVolume);
        
        SfxVolume = PlayerPrefs.GetFloat(PLAYERPREFS_SFXVOLUME, -8);
        _masterMixer.SetFloat("SfxVolume", SfxVolume);
    }

    public static float ConvertDbToPercentage(float db)
    {
        return ((db + 80) / 80) * 100f;
    }
    private void Start()
    {
        PlayBgm(_testClip);
        Tile.OnTile2048 += Play2048;
        Tile.OnTileMerge += Tile_OnTileMerge;
        MS.Main.GameManager.GameLost += PlayGameOver;
        BoardPlayerInput.Instance.OnValidInputReceived += BoardPlayerInput_OnValidInputReceived;
    }

    private void BoardPlayerInput_OnValidInputReceived(Vector2 obj)
    {
        PlaySwish();
    }

    private void Tile_OnTileMerge(int obj)
    {
        PlayMerge();
    }


    public void PlayBgm(AudioClip clip) {
        if (_bgmSource.isPlaying && _bgmSource.clip == clip) {
            return;
        }
        
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }
    
    public void StopBgm() {
        _bgmSource.Stop();
    }
    
    public void PlaySfx(AudioClip clip) {
        _sfxSource.pitch = Random.Range(0.5f, 0.55f);
        _sfxSource.PlayOneShot(clip);
    }
    
    public void PlaySfxFromArray(AudioClip[] clips)
    {
        var clip = clips[Random.Range(0, clips.Length)];
        _sfxSource.pitch = Random.Range(0.5f, 0.55f);
        _sfxSource.PlayOneShot(clip);
    }
    
    public void ChangeMasterVolume() {
        MasterVolume += 4f;
        MasterVolume = MasterVolume > 0 ? -80 : MasterVolume;
        _masterMixer.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat(PLAYERPREFS_MASTERVOLUME, MasterVolume);
        PlayerPrefs.Save();
    }
    
    public void ChangeMusicVolume() {
        MusicVolume += 4f;
        MusicVolume = MusicVolume > 0 ? -80 : MusicVolume;
        _masterMixer.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat(PLAYERPREFS_MUSICVOLUME, MusicVolume);
        PlayerPrefs.Save();
    }
    
    public void ChangeSfxVolume() {
        SfxVolume += 4f;
        SfxVolume = SfxVolume > 0 ? -80 : SfxVolume;
        _masterMixer.SetFloat("SfxVolume", SfxVolume);
        PlayerPrefs.SetFloat(PLAYERPREFS_SFXVOLUME, SfxVolume);
        PlayerPrefs.Save();
    }
    
}
