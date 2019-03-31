using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterPlay : MonoBehaviour {

    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioOptions audioOptions;

    [SerializeField]
    bool playOnStart = true;

	void Start () {
        GameFlowManager.instance.audioManager.RegisterAudioSource(source);

        if (playOnStart)
        {
            Play();
        }
	}

    public void Play()
    {
        GameFlowManager.instance.audioManager.Play(source.name, source.clip, audioOptions);
    }
}
