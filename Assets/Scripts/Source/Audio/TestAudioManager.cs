using UnityEngine;

public class TestAudioManager : MonoBehaviour 
{
    [SerializeField]
    AudioSource musicSource;

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    AudioClip music;

	void Start()
    {
        audioManager.RegisterAudioSource(musicSource);
        audioManager.Play("MusicSource", music);
    }
}
