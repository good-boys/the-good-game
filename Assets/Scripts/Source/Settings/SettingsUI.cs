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
        musicSlider.value = settings.MusicVolume;
        musicSlider.onValueChanged.AddListener(delegate
        {
            settings.MusicVolume = musicSlider.value;
        });
        sfxSlider.value = settings.SFXVolume;
        sfxSlider.onValueChanged.AddListener(delegate
        {
            settings.SFXVolume = sfxSlider.value;
        });
    }
}
