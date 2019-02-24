using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour 
{
    Dictionary<string, AudioSource> sources = new Dictionary<string, AudioSource>();
    Dictionary<string, AudioOptions> clipOptions = new Dictionary<string, AudioOptions>();
    Dictionary<string, List<IEnumerator>> clipCoroutinesMap = new Dictionary<string, List<IEnumerator>>();

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
        clipOptions[getClipIdentifier(source, clip)] = options;
        handleClipTransition(source, clip, options);
    }

    void handleClipTransition(AudioSource source, AudioClip newClip, AudioOptions options)
    {
        AudioClip previousClip = source.clip;
        string previousClipId = previousClip ? getClipIdentifier(source, previousClip) : null;
        AudioOptions previousClipOptions;
        if(!previousClip || !clipOptions.TryGetValue(previousClipId, out previousClipOptions))
        {
            previousClipOptions = AudioOptions.GetDefault();
        }
        IEnumerator fadeInCoroutine = null;
        IEnumerator endClipCoroutine = null;
        Action endHandlerAction = delegate{ options.OnEnd(); };
        if(options.HasFadeIn)
        {
            fadeInCoroutine = getFadeInCoroutine(source, options);
        }
        if(options.HasFadeOut)
        {
            IEnumerator fadeOutCoroutine = getFadeOutCoroutine(source, options, options.HasEndHandler ? endHandlerAction : null);
            endClipCoroutine = waitAndExecute(newClip.length - options.FadeOutTime, delegate
            {
                StartCoroutine(fadeOutCoroutine);
            });
        }
        else if(options.HasEndHandler)
        {
            endClipCoroutine = waitAndExecute(newClip.length, endHandlerAction);
        }
        Action playClipAction = delegate
        {
            previousClipOptions.OnEnd();
            source.clip = newClip;
            if(fadeInCoroutine != null)
            {
                source.volume = 0;
                StartCoroutine(fadeInCoroutine);
            }
            source.Play();
        };
        if(previousClip)
        {
            cleanUpClipCoroutines(source, previousClip);
        }
        if(options.HardTransitionIn || !previousClip || !previousClipOptions.HasFadeOut)
        {
            playClipAction();
        }
        else
        {
            IEnumerator fadeOutPrevious = getFadeOutCoroutine(source, previousClipOptions, playClipAction);
            StartCoroutine(fadeOutPrevious);
        }
    }

    IEnumerator getFadeInCoroutine(AudioSource source, AudioOptions options, Action fadeEndHandler = null)
    {
        return getFadeCoroutine(source, 0, options.Volume, options.FadeInTime, fadeEndHandler);
    }

    IEnumerator getFadeOutCoroutine(AudioSource source, AudioOptions options, Action fadeEndHandler = null)
    {
        return getFadeCoroutine(source, source.volume, 0, options.FadeOutTime, fadeEndHandler);
    }

    IEnumerator getFadeCoroutine(AudioSource source, 
                                 float startVolume, 
                                 float endVolume, 
                                 float time, 
                                 Action fadeEndHandler = null)
    {
        float timer = 0;
        while(timer <= time)
        {
            source.volume = Mathf.Lerp(startVolume, endVolume, timer / time);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        if(fadeEndHandler != null)
        {
            fadeEndHandler();
        }
    }

    IEnumerator waitAndExecute(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    void cleanUpClipCoroutines(AudioSource source, AudioClip clip)
    {
        string clipIdentifier = getClipIdentifier(source, clip);
        List<IEnumerator> clipCoroutines;
        if(clipCoroutinesMap.TryGetValue(clipIdentifier, out clipCoroutines))
        {
            foreach(IEnumerator coroutine in clipCoroutines)
            {
                StopCoroutine(coroutine);
            }
            clipCoroutinesMap.Remove(clipIdentifier);
        }
    }

    string getClipIdentifier(AudioSource source, AudioClip clip)
    {
        Debug.Log(string.Format("{0}{1}", source.name, clip.name));
        return string.Format("{0}{1}", source.name, clip.name);
    }
}
