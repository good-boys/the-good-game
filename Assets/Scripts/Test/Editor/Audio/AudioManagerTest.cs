using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;

public class AudioManagerTest
{
    const string MOCK_AUDIO_SOURCE_NAME = "MOCK_AUDIOSOURCE";
    const string MOCK_AUDIO_CLIP_NAME = "MOCK_AUDIOCLIP";

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
        audioClip = AudioClip.Create(MOCK_AUDIO_CLIP_NAME, 1, 1, 1, false);
        audioSourceGameObject = new GameObject(MOCK_AUDIO_SOURCE_NAME);
        audioSource = audioSourceGameObject.AddComponent<AudioSource>();

        gameObject = new GameObject();

        audioManager = gameObject.AddComponent<AudioManager>();
        audioManager.Init(sources);
    }

    [TearDown]
    public void Teardown()
    {
        UnityConsole.Clear();
    }

    [Test]
    public void TestRegisterAudioSource()
    {
        audioManager.RegisterAudioSource(audioSource);

        Assert.AreEqual(MOCK_AUDIO_SOURCE_NAME, sources.ElementAt(0).Key);
        Assert.AreEqual(audioSource, sources[MOCK_AUDIO_SOURCE_NAME]);
    }

    [UnityTest]
    public IEnumerator TestPlay()
    {
        audioManager.RegisterAudioSource(audioSource);

        audioManager.Play(MOCK_AUDIO_SOURCE_NAME, audioClip);
        yield return null;

        Assert.True(audioSource.isPlaying);
        Assert.AreEqual(audioClip, audioSource.clip);
    }

    [UnityTest]
    public IEnumerator TestPlay_NotFound()
    {
        const string OTHER_SOURCE = "OTHER_SOURCE";

        audioManager.RegisterAudioSource(audioSource);
        LogAssert.Expect(LogType.Error, new Regex(OTHER_SOURCE));

        audioManager.Play(OTHER_SOURCE, audioClip);
        yield return null;

        Assert.False(audioSource.isPlaying);
        Assert.Null(audioSource.clip);
    }

    [UnityTest]
    public IEnumerator TestGetPlayingAudioSources()
    {
        audioManager.RegisterAudioSource(audioSource);
        audioManager.Play(MOCK_AUDIO_SOURCE_NAME, audioClip);
        yield return null;

        List<AudioSource> sources = audioManager.GetPlayingSources();

        Assert.AreEqual(1, sources.Count);
        Assert.AreEqual(audioSource, sources[0]);
    }

    [UnityTest]
    public IEnumerator TestGetPlayingClips()
    {
        audioManager.RegisterAudioSource(audioSource);
        audioManager.Play(MOCK_AUDIO_SOURCE_NAME, audioClip);
        yield return null;

        List<AudioClip> clips = audioManager.GetPlayingClips();

        Assert.AreEqual(1, clips.Count);
        Assert.AreEqual(audioClip, clips[0]);
    }
}
