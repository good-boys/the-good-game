using System;
using UnityEngine;

public class SettingsManager
{
    public const float MAX_VOLUME = 1f;
    public const float MIN_VOLUME = 0.001f;

    private static Action<float> onMusicChange = delegate(float vol) {};
    private static Action<float> onSFXChange = delegate (float vol) { };

    ISettingsSerializer settingsSerializer;
    Action<float> musicChangeDelegate = delegate (float vol) { };
    Action<float> sfxChangeDelegate = delegate (float vol) { };

    public SettingsManager(ISettingsSerializer settingsSerializer)
    {
        this.settingsSerializer = settingsSerializer;
        handleMusicChange(settingsSerializer.RetrieveMusicVolume());
        handleSFXChange(settingsSerializer.RetrieveSFXVolume());
        onMusicChange += handleMusicChange;
        onSFXChange += handleSFXChange;
    }

    ~SettingsManager()
    {
        onMusicChange -= handleMusicChange;
        onSFXChange -= handleSFXChange;
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
            settingsSerializer.SerializeMusicVolume(value);
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
            settingsSerializer.SerializeSFXVolume(value);
            onSFXChange(value);
        }
    }

    float _musicVolume, _sfxVolume;

    private void handleMusicChange(float volume)
    {
        _musicVolume = Mathf.Clamp(volume, MIN_VOLUME, MAX_VOLUME);
        musicChangeDelegate(volume);
    }

    private void handleSFXChange(float volume)
    {
        _sfxVolume = Mathf.Clamp(volume, MIN_VOLUME, MAX_VOLUME);
        sfxChangeDelegate(volume);
    }

    public void SubscribeMusicChange(Action<float> handleMusicChange)
    {
        musicChangeDelegate += handleMusicChange;
    }

    public void SubscribeSFXChange(Action<float> handleSFXChange)
    {
        sfxChangeDelegate += handleSFXChange;
    }
}
