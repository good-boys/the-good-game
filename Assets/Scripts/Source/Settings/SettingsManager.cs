using System;
using UnityEngine;

public class SettingsManager
{
    private static Action<float> onMusicChange = delegate(float vol) {};
    private static Action<float> onSFXChange = delegate (float vol) { };

    const float MAX_VOLUME = 1f;
    const float MIN_VOLUME = 0.001f;

    ISettingsSerializer settingsSerializer;

    public SettingsManager(ISettingsSerializer settingsSerializer)
    {
        this.settingsSerializer = settingsSerializer;
        handleMusicChange(settingsSerializer.RetrieveMusicVolume());
        handleSFXChange(settingsSerializer.RetrieveSFXVolume());
        onMusicChange += handleMusicChange;
        onSFXChange += handleSFXChange;
    }

    public float MusicVolume
    {
        get
        {
            return _musicVolume;
        }
        set
        {
            value = Mathf.Clamp(value, MIN_VOLUME, MAX_VOLUME);
            settingsSerializer.SerializeMusicVolume(_musicVolume);
            onMusicChange(value);
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
            value = Mathf.Clamp(value, MIN_VOLUME, MAX_VOLUME);
            settingsSerializer.SerializeSFXVolume(_sfxVolume);
            onSFXChange(value);
        }
    }

    float _musicVolume, _sfxVolume;

    private void handleMusicChange(float volume)
    {
        _musicVolume = Mathf.Clamp(volume, MIN_VOLUME, MAX_VOLUME);
    }

    private void handleSFXChange(float volume)
    {
        _sfxVolume = Mathf.Clamp(volume, MIN_VOLUME, MAX_VOLUME);
    }
}
