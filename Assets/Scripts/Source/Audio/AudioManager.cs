using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
    Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();
    Dictionary<AudioClip, AudioOptions> clipOptions = new Dictionary<AudioClip, AudioOptions>();

    public void Init(Dictionary<string, AudioSource> sources)
    {
        this.sources = sources;
    }

    public void RegisterAudioSource(AudioSource source)
    {
        sources[source.gameObject.name] = source;
    }

    public void Play(string sourceName, AudioClip clip, AudioOptions options=null)
    {
        AudioSource source;
        if(!sources.TryGetValue(sourceName, out source))
        {
            Debug.LogErrorFormat("Source {0} not found. Unable to play clip.", sourceName);
            return;
        }
        if(options == null)
        {
            options = AudioOptions.GetDefault();
        }

        source.clip = clip;
        source.Play();
    }
}
