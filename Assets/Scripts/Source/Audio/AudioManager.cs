using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
    Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();

    public void RegisterAudioSource(AudioSource source)
    {
        sources[source.gameObject.name] = source;
    }

    public void Play(string sourceName, AudioClip clip)
    {
        AudioSource source;
        if(!sources.TryGetValue(sourceName, out source))
        {
            Debug.LogError("Source {0} not found. Unable to play clip.");
            return;
        }

        source.clip = clip;
        source.Play();
    }
}
