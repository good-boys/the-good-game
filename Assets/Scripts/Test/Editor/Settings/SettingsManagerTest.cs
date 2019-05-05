using NUnit.Framework;
using Moq;

public class SettingsManagerTest
{
    Mock<ISettingsSerializer> mockSerializer;
    SettingsManager settings;
    float musicVolume = 0.75f;
    float sfxVolume = 0.23f;

    [SetUp]
    public void Setup()
    {
        mockSerializer = new Mock<ISettingsSerializer>();
        mockSerializer.Setup(s => s.RetrieveMusicVolume()).Returns(musicVolume);
        mockSerializer.Setup(s => s.RetrieveSFXVolume()).Returns(sfxVolume);
        settings = new SettingsManager(mockSerializer.Object);
    }

    [Test]
    public void TestInit()
    {
        mockSerializer = new Mock<ISettingsSerializer>();
        mockSerializer.Setup(s => s.RetrieveMusicVolume()).Returns(musicVolume);
        mockSerializer.Setup(s => s.RetrieveSFXVolume()).Returns(sfxVolume);

        settings = new SettingsManager(mockSerializer.Object);

        mockSerializer.Verify(s => s.RetrieveMusicVolume());
        mockSerializer.Verify(s => s.RetrieveSFXVolume());
    }

    [Test]
    public void TestGetMusicVolume()
    {
        Assert.AreEqual(musicVolume, settings.MusicVolume);
    }

    [Test]
    public void TestSetMusicVolume()
    {
        float newMusicVolume = 0.85f;

        settings.MusicVolume = newMusicVolume;

        Assert.AreEqual(newMusicVolume, settings.MusicVolume);
        mockSerializer.Verify(s => s.SerializeMusicVolume(newMusicVolume));
    }

    [Test]
    public void TestMusicVolume_Clamp()
    {
        float newMusicVolume = -0.1f;

        settings.MusicVolume = newMusicVolume;

        Assert.AreEqual(0, settings.MusicVolume);
        mockSerializer.Verify(s => s.SerializeMusicVolume(0));
    }

    [Test]
    public void TestGetSFXVolume()
    {
        Assert.AreEqual(sfxVolume, settings.SFXVolume);
    }

    [Test]
    public void TestSetSFXVolume()
    {
        float newSFXVolume = 0.15f;

        settings.SFXVolume = newSFXVolume;

        Assert.AreEqual(newSFXVolume, settings.SFXVolume);
        mockSerializer.Verify(s => s.SerializeSFXVolume(newSFXVolume));
    }

    [Test]
    public void TestSetSFXVolume_Clamp()
    {
        float newSFXVolume = 1.44f;

        settings.SFXVolume = newSFXVolume;

        Assert.AreEqual(1, settings.SFXVolume);
        mockSerializer.Verify(s => s.SerializeSFXVolume(1));
    }
}