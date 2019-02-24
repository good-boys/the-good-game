using UnityEngine;

public class DemoAudioManagerUsage : MonoBehaviour 
{
    [SerializeField]
    AudioSource musicSource;

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    AudioClip music;

    [SerializeField]
    float fadeInTime;

    void Start()
    {
        AudioOptions options = new AudioOptions(fadeInTime);
        audioManager.RegisterAudioSource(musicSource);
        audioManager.Play("MusicSource", music, options);
    }
}
