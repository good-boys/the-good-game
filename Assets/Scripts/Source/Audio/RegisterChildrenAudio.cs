using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterChildrenAudio : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
        {
            GameFlowManager.instance.audioManager.RegisterAudioSource(source);
        }
	}
}
