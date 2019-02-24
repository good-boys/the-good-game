using System;

public class AudioOptions 
{
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

    public float FadeInTime { get; private set; }
    public float FadeOutTime { get; private set; }
    public bool Looping { get; private set; }
    // Should the AudioSource cutover without fading out the AudioClip?
    public bool HardTransitionIn { get; private set; }

    private Action onEndHandler;

    public AudioOptions(float fadeInTime = 0f,
                        float fadeOutTime = 0f,
                        bool shouldLoop = false,
                        bool hardTransitionIn = true,
                        Action onEndHandler = null)
    {
        this.FadeInTime = fadeInTime;
        this.FadeOutTime = FadeOutTime;
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
