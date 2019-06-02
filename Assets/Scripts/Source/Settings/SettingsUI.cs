using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour 
{
    [SerializeField]
    Slider musicSlider, sfxSlider;

    SettingsManager settings;

    private void Awake()
    {
        ISettingsSerializer serializer = new UnitySettingsSerializer();
        Init(new SettingsManager(serializer));
    }

    public void Init(SettingsManager settings)
    {
        this.settings = settings;
        musicSlider.maxValue = SettingsManager.MAX_VOLUME;
        musicSlider.minValue = SettingsManager.MIN_VOLUME;
        musicSlider.value = settings.MusicVolume;
        musicSlider.onValueChanged.AddListener(delegate
        {
            settings.MusicVolume = musicSlider.value;
        });
        sfxSlider.maxValue = SettingsManager.MAX_VOLUME;
        sfxSlider.minValue = SettingsManager.MIN_VOLUME;
        sfxSlider.value = settings.SFXVolume;
        sfxSlider.onValueChanged.AddListener(delegate
        {
            settings.SFXVolume = sfxSlider.value;
        });
    }
}
