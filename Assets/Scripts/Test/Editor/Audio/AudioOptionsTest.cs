using System;
using NUnit.Framework;

public class AudioOptionsTest
{
    [Test]
    public void TestInit()
    {
        float fadeInTime = 0.3f;
        float fadeOutTime = 0.0f;
        bool shouldLoop = true;
        bool hardTransitionIn = false;
        Action onEndHandler = delegate {};

        AudioOptions options = new AudioOptions(
            fadeInTime,
            fadeOutTime,
            shouldLoop,
            hardTransitionIn,
            onEndHandler);

        Assert.AreEqual(fadeInTime, options.FadeInTime);
        Assert.AreEqual(fadeOutTime, options.FadeOutTime);
        Assert.AreEqual(shouldLoop, options.Looping);
        Assert.AreEqual(hardTransitionIn, options.HardTransitionIn);
        Assert.True(options.HasFadeIn);
        Assert.False(options.HasFadeOut);
    }

    [Test]
    public void TestOnEnd()
    {
        float fadeInTime = 0.3f;
        float fadeOutTime = 0.0f;
        bool shouldLoop = true;
        bool hardTransitionIn = false;
        bool handlerRun = false;
        Action onEndHandler = delegate 
        {
            handlerRun = true;
        };
        AudioOptions options = new AudioOptions(
            fadeInTime,
            fadeOutTime,
            shouldLoop,
            hardTransitionIn,
            onEndHandler);

        options.OnEnd();

        Assert.True(handlerRun);
    }

    [Test]
    public void TestGetDefault()
    {
        AudioOptions options = AudioOptions.GetDefault();

        Assert.AreEqual(0, options.FadeInTime);
        Assert.AreEqual(0, options.FadeOutTime);
        Assert.False(options.Looping);
        Assert.True(options.HardTransitionIn);
        Assert.False(options.HasFadeIn);
        Assert.False(options.HasFadeOut);
    }
}
