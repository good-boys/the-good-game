﻿using UnityEngine;

public class UnitySettingsSerializer : ISettingsSerializer 
{
    const string MUSIC_VOLUME = "MUSIC_VOLUME";
    const string SFX_VOLUME = "SFX_VOLUME";

    public float RetrieveMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME);
    }

    public float RetrieveSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME);
    }

    public void SerializeMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
    }

    public void SerializeSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
    }
}
