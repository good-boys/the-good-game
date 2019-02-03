using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager
{
    ISettingsSerializer settingsSerializer;

    public SettingsManager(ISettingsSerializer settingsSerializer)
    {
        this.settingsSerializer = settingsSerializer;
        _musicVolume = settingsSerializer.RetrieveMusicVolume();
        _sfxVolume = settingsSerializer.RetrieveSFXVolume();
    }

    public float MusicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            _musicVolume = Mathf.Clamp(value, 0, 1);
            settingsSerializer.SerializeMusicVolume(_musicVolume);
        }
    }

    public float SFXVolume
    {
        get
        {
            return _sfxVolume;
        }
        set
        {
            _sfxVolume = Mathf.Clamp(value, 0, 1);
            settingsSerializer.SerializeSFXVolume(_sfxVolume);
        }
    }

    float _musicVolume, _sfxVolume;
}
