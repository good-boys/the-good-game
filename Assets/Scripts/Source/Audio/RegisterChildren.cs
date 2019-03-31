using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterChildren : MonoBehaviour {

	// Use this for initialization
	void Start () {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameFlowManager.instance.audioManager.RegisterAudioSource(transform.GetChild(i).GetComponent<AudioSource>());
        }
	}
}
