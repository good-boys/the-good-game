using UnityEngine;

public class DemoAudioManagerUsage : MonoBehaviour 
{
    [SerializeField]
    AudioClip[] clipOptions = new AudioClip[3];

    [SerializeField]
    AudioSource audioSource;

    [SerializeField]
    AudioManager audioManager;

    [SerializeField]
    AudioClip activeClip;

    [SerializeField]
    float fadeInTime, fadeOutTime;

    [SerializeField]
    [Range(0, 1)]
    float volume = 1f;

    [SerializeField]
    bool looping;

    [SerializeField]
    bool hardTransitionIn;

    [SerializeField]
    string messageOnEnd = "Clip done playing";

    [SerializeField]
    KeyCode playClipKeyCode = KeyCode.Space;

    void Start()
    {
        playClip();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            activeClip = clipOptions[0];
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            activeClip = clipOptions[1];
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            activeClip = clipOptions[2];
        }
        if (Input.GetKeyDown(playClipKeyCode))
        {
            playClip();
        }
    }

    void playClip()
    {
        audioManager.RegisterAudioSource(audioSource);
        audioManager.Play(gameObject.name, activeClip, getOptions());
    }

    AudioOptions getOptions()
    {
        return new AudioOptions(fadeInTime,
                                fadeOutTime,
                                volume,
                                looping,
                                hardTransitionIn,
                                delegate { Debug.Log(messageOnEnd); });
    }
}
