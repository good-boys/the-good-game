﻿using System;
using NUnit.Framework;
using Moq;
using System.Collections.Generic;

public class DataInitializerTest : MonoBehaviorTestBase<DataInitializer>
{
    string playerName;
    string saveFile;
    int seed;
    int playerHealth;
    int playerSpeed;
    int currentlevel;
    int currentweapon;

    CharacterConfig config;
    Player newPlayer;

    Mock<SaveManager> mockSaveManager;
    Mock<GameSave> mockGameSave;
    
    [SetUp]
    public override void Setup()
    {
        saveFile = "unit_test.dat";
        seed = 23;
        playerName = "player name";
        playerHealth = 75;
        playerSpeed = 21;
        currentweapon = 0;
        currentlevel = 0;

        config = new CharacterConfig(playerName, playerHealth, playerSpeed);
        newPlayer = new Player(config);

        mockSaveManager = new Mock<SaveManager>(saveFile);
        mockGameSave = new Mock<GameSave>(seed, newPlayer, currentweapon, currentlevel);

        base.Setup();

        testInstance.Init(saveFile, seed, config, mockSaveManager.Object, currentweapon, currentlevel, new Dictionary<string, Tutorial>(){
            { Tutorial.ATTACK_TUTORIAL, new Mock<Tutorial>().Object },
            { Tutorial.DEFEND_TUTORIAL, new Mock<Tutorial>().Object },
            { Tutorial.COMBO_TUTORIAL, new Mock<Tutorial>().Object }
         });

        mockSaveManager.Setup(manager => manager.Load()).Returns(mockGameSave.Object);
    }

    [Test]
    public void TestGetGameSave()
    {
        mockSaveManager.Setup(manager => manager.HasSave()).Returns(true);

        testInstance.Awake();

        Assert.AreSame(mockGameSave.Object, testInstance.GameSave);
    }

    [Test]
    public void TestGetSaveManager()
    {
        Assert.AreSame(mockSaveManager.Object, testInstance.SaveManager);
    }

    [Test]
    public void TestAwake_HasSave()
    {
        mockSaveManager.Setup(manager => manager.HasSave()).Returns(true);

        testInstance.Awake();

        Assert.AreSame(mockGameSave.Object, testInstance.GameSave);
        mockSaveManager.Verify(manager => manager.Load());
    }

    [Test]
    public void TestAwake_NoSave()
    {
        testInstance.Awake();

        Assert.AreEqual(new Random(seed).Next(), 
                        testInstance.GameSave.Random.Next());
        Assert.AreEqual(newPlayer.Name, testInstance.GameSave.Player.Name);
        Assert.AreEqual(newPlayer.Health, testInstance.GameSave.Player.Health);
        Assert.AreEqual(newPlayer.Speed, testInstance.GameSave.Player.Speed);
        mockSaveManager.Verify(manager => manager.Load(), Times.Never());
    }

    [Test]
    public void TestIsInitialized_HasSave()
    {
        testInstance.Awake();

        Assert.True(testInstance.IsInitialized());
    }

    [Test]
    public void TestIsInitialized_NoSave()
    {
        Assert.False(testInstance.IsInitialized());
    }
}
