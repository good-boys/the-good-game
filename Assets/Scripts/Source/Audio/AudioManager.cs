﻿using System;
using System.Linq;
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

        if (clip == null)
        {
            Debug.LogErrorFormat("Clip {0} not found.", sourceName);
            return;
        }
        clipOptions[getClipIdentifier(source, clip)] = options;
        handleClipTransition(source, clip, options);
    }

    public void Play(string sourceName, AudioOptions options = null)
    {
        AudioSource source;
        if (!sources.TryGetValue(sourceName, out source))
        {
            Debug.LogErrorFormat("Source {0} not found. Unable to stop clip.", sourceName);
            return;
        }

        AudioClip clip = source.clip;
        if (clip == null)
        {
            Debug.LogErrorFormat("Clip {0} not found.", sourceName);
            return;
        }

        Play(sourceName, clip, options);
    }

    public void Stop(string sourceName, AudioClip clip)
    {
        AudioSource source;
        if (!sources.TryGetValue(sourceName, out source))
        {
            Debug.LogErrorFormat("Source {0} not found. Unable to stop clip.", sourceName);
            return;
        }

        if (!clipOptions.ContainsKey(getClipIdentifier(source, clip)))
        {
            Debug.LogErrorFormat("Source {0} options not found. Unable to stop clip.", sourceName);
            return;
        }

        AudioOptions options = clipOptions[getClipIdentifier(source, clip)];
        Action endHandlerAction = delegate { options.OnEnd(); };
        IEnumerator fadeOutCoroutine = getFadeOutCoroutine(source, options, options.HasEndHandler ? endHandlerAction : null);
        if (options.HasEndHandler)
        {
            IEnumerator endClipCoroutine = waitAndExecute(Mathf.Max(0, options.FadeOutTime), delegate
            {
                StartCoroutine(fadeOutCoroutine);
            });
        }
        else
        {
            StartCoroutine(fadeOutCoroutine);
        }
    }

    public void Stop(string sourceName)
    {
        AudioSource source;
        if (!sources.TryGetValue(sourceName, out source))
        {
            Debug.LogErrorFormat("Source {0} not found. Unable to stop clip.", sourceName);
            return;
        }

        AudioClip clip = source.clip;
        if (clip == null)
        {
            Debug.LogErrorFormat("Clip {0} not found.", sourceName);
            return;
        }

        Stop(sourceName, clip);
    }

    public List<AudioSource> GetPlayingSources()
    {
        return sources.Values.Where(source => source.isPlaying).ToList();
    }

    public List<AudioClip> GetPlayingClips()
    {
        return GetPlayingSources().Select(source => source.clip).ToList();
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
        IEnumerator loopClipCoroutine = null;
        Action endHandlerAction = delegate{ options.OnEnd(); };
        if(options.HasFadeIn)
        {
            fadeInCoroutine = getFadeInCoroutine(source, options);
        }
        if(options.HasFadeOut)
        {
            IEnumerator fadeOutCoroutine = getFadeOutCoroutine(source, options, options.HasEndHandler ? endHandlerAction : null);
            endClipCoroutine = waitAndExecute(Mathf.Max(0, newClip.length - options.FadeOutTime), delegate
            {
                StartCoroutine(fadeOutCoroutine);
            });
        }
        else if(options.HasEndHandler)
        {
            endClipCoroutine = waitAndExecute(newClip.length, endHandlerAction);
        }
        if(options.Looping)
        {
            loopClipCoroutine = waitAndExecute(newClip.length, delegate
            {
                Play(source.name, newClip, options);
            });
        }
        Action playClipAction = delegate
        {
            previousClipOptions.OnEnd();
            source.clip = newClip;
            if(fadeInCoroutine != null)
            {
                source.volume = 0;
                StartCoroutine(fadeInCoroutine);
                registerClipCoroutine(source, newClip, fadeInCoroutine);
            }
            if(endClipCoroutine != null)
            {
                StartCoroutine(endClipCoroutine);
                registerClipCoroutine(source, newClip, endClipCoroutine);
            }
            if(loopClipCoroutine != null)
            {
                StartCoroutine(loopClipCoroutine);
                registerClipCoroutine(source, newClip, loopClipCoroutine);
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
            registerClipCoroutine(source, newClip, fadeOutPrevious);
        }
    }

    IEnumerator getFadeInCoroutine(AudioSource source, AudioOptions options, Action fadeEndHandler = null)
    {
        return getFadeCoroutine(source, options.Volume, options.FadeInTime, fadeEndHandler);
    }

    IEnumerator getFadeOutCoroutine(AudioSource source, AudioOptions options, Action fadeEndHandler = null)
    {
        return getFadeCoroutine(source, 0, options.FadeOutTime, fadeEndHandler);
    }

    IEnumerator getFadeCoroutine(AudioSource source, 
                                 float endVolume, 
                                 float time, 
                                 Action fadeEndHandler = null)
    {
        float startVolume = source.volume;
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
        source.volume = endVolume;
    }

    IEnumerator waitAndExecute(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }

    void registerClipCoroutine(AudioSource source, 
                               AudioClip clip, 
                               IEnumerator coroutine)
    {
        List<IEnumerator> clipCoroutines;
        string clipIdentifier = getClipIdentifier(source, clip);
        if(!clipCoroutinesMap.TryGetValue(clipIdentifier, out clipCoroutines))
        {
            clipCoroutines = new List<IEnumerator>();
            clipCoroutinesMap[clipIdentifier] = clipCoroutines;
        }
        clipCoroutines.Add(coroutine);
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
        return string.Format("{0}{1}", source.name, clip.name);
    }
}
