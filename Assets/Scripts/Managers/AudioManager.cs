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




    public float MusicVolume { get; private set; }
    private const string PLAYERPREFS_MUSICVOLUME = "MusicVolume";
    
    public float MasterVolume { get; private set; }
    private const string PLAYERPREFS_MASTERVOLUME = "MasterVolume";
    
    public float SfxVolume { get; private set; }
    private const string PLAYERPREFS_SFXVOLUME = "SfxVolume";

    private void Awake()
    {
     
        MusicVolume = PlayerPrefs.GetFloat(PLAYERPREFS_MUSICVOLUME, 5);
        _masterMixer.SetFloat("MusicVolume", MusicVolume);
        
        MasterVolume = PlayerPrefs.GetFloat(PLAYERPREFS_MASTERVOLUME, 5);
        _masterMixer.SetFloat("MasterVolume", MasterVolume);
        
        SfxVolume = PlayerPrefs.GetFloat(PLAYERPREFS_SFXVOLUME, 5);
        _masterMixer.SetFloat("SfxVolume", SfxVolume);
    }

    
    private void Start()
    {
        PlayBgm(_testClip);
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
    
    public void ChangeMasterVolume() {
        MasterVolume += 5f;
        MasterVolume = MasterVolume > 10 ? -80 : MasterVolume;
        _masterMixer.SetFloat("MasterVolume", MasterVolume);
        PlayerPrefs.SetFloat(PLAYERPREFS_MASTERVOLUME, MasterVolume);
        PlayerPrefs.Save();
    }
    
    public void ChangeMusicVolume() {
        MusicVolume += 5f;
        MusicVolume = MusicVolume > 10 ? -80 : MusicVolume;
        _masterMixer.SetFloat("MusicVolume", MusicVolume);
        PlayerPrefs.SetFloat(PLAYERPREFS_MUSICVOLUME, MusicVolume);
        PlayerPrefs.Save();
    }
    
    public void ChangeSfxVolume() {
        SfxVolume += 5f;
        SfxVolume = SfxVolume > 10 ? -80 : SfxVolume;
        _masterMixer.SetFloat("SfxVolume", SfxVolume);
        PlayerPrefs.SetFloat(PLAYERPREFS_SFXVOLUME, SfxVolume);
        PlayerPrefs.Save();
    }
    
}
