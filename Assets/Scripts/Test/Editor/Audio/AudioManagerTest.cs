using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;
using NUnit.Framework;

public class AudioManagerTest
{
    const string MOCK_AUDIO_SOURCE_NAME = "MOCK_AUDIOSOURCE";

    Dictionary<string, AudioSource> sources;
    AudioSource audioSource;
    AudioClip audioClip;
    GameObject audioSourceGameObject;
    GameObject gameObject;
    AudioManager audioManager;

    [SetUp]
    public void Setup()
    {
        sources = new Dictionary<string, AudioSource>();
        audioClip = new AudioClip();
        audioSourceGameObject = new GameObject(MOCK_AUDIO_SOURCE_NAME);
        audioSource = audioSourceGameObject.AddComponent<AudioSource>();

        gameObject = new GameObject();

        audioManager = gameObject.AddComponent<AudioManager>();
        audioManager.Init(sources);
    }

    [Test]
    public void TestRegisterAudioSource()
    {
        audioManager.RegisterAudioSource(audioSource);

        Assert.AreEqual(MOCK_AUDIO_SOURCE_NAME, sources.ElementAt(0).Key);
        Assert.AreEqual(audioSource, sources[MOCK_AUDIO_SOURCE_NAME]);
    }

    [Test]
    public void TestPlay()
    {
        audioManager.RegisterAudioSource(audioSource);

        audioManager.Play(MOCK_AUDIO_SOURCE_NAME, audioClip);

        Assert.AreEqual(audioClip, audioSource.clip);
    }

    [Test]
    public void TestPlay_NotFound()
    {
        const string OTHER_SOURCE = "OTHER_SOURCE";

        audioManager.RegisterAudioSource(audioSource);
        LogAssert.Expect(LogType.Error, new Regex(OTHER_SOURCE));

        audioManager.Play(OTHER_SOURCE, audioClip);

        Assert.Null(audioSource.clip);
    }
}
