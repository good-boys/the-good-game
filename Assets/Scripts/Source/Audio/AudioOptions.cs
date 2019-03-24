using System;

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

    public float FadeInTime { get; private set; }
    public float FadeOutTime { get; private set; }
    public float Volume { get; private set; }
    public bool Looping { get; private set; }
    // Should the AudioSource cutover without fading out the AudioClip?
    public bool HardTransitionIn { get; private set; }

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
