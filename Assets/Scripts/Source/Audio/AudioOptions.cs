using System;
using UnityEngine;

[Serializable]
public class AudioOptions
{
    private const float MAX_VOLUME = 1f;
    private const float MIN_VOLUME = 0f;

    public bool HasFadeIn
    {
        get
        {
            return FadeInTime > 0;
        }
    }

    public bool HasFadeOut
    {
        get
        {
            return FadeOutTime > 0;
        }
    }

    public bool HasEndHandler
    {
        get
        {
            return onEndHandler != null;
        }
    }

    public float FadeInTime { get { return fadeIn; } private set { fadeIn = value; } }
    [SerializeField]
    private float fadeIn;

    public float FadeOutTime { get { return fadeOut; } private set { fadeOut = value; } }
    [SerializeField]
    private float fadeOut;

    public float Volume { get { return vol; } private set { vol = value; } }
    [SerializeField]
    private float vol;

    public bool Looping { get { return looping; } private set { looping = value; } }
    [SerializeField]
    private bool looping;

    public bool HardTransitionIn { get { return hardTransition; } private set { hardTransition = value; } }
    [SerializeField]
    private bool hardTransition;

    private Action onEndHandler;

    public AudioOptions(float fadeInTime = 0f,
                        float fadeOutTime = 0f,
                        float volume = 1f,
                        bool shouldLoop = false,
                        bool hardTransitionIn = true,
                        Action onEndHandler = null)
    {
        this.FadeInTime = fadeInTime;
        this.FadeOutTime = fadeOutTime;
        this.Volume = Math.Max(MIN_VOLUME, Math.Min(MAX_VOLUME, volume));
        this.Looping = shouldLoop;
        this.HardTransitionIn = hardTransitionIn;
        this.onEndHandler = onEndHandler;
    }

    public void OnEnd()
    {
        if(onEndHandler == null)
        {
            return;
        }

        onEndHandler();
    }

    public static AudioOptions GetDefault()
    {
        return new AudioOptions();
    }
}
