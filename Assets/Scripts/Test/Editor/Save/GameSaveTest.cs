using System;
using NUnit.Framework;

public class GameSaveTest
{
    int seed;
    int startingPlayerHealth = 81;
    Player testPlayer;
    
    [SetUp]
    public void Setup()
    {
        seed = 85;
        testPlayer = new Player("test player", startingPlayerHealth, 2);
    }

    [Test]
    public void TestInit()
    {
        GameSave save = new GameSave(seed, testPlayer);

        Assert.AreEqual(new Random(seed).Next(), save.Random.Next());
        Assert.AreSame(testPlayer, save.Player);
    }

    [Test]
    public void TestReset()
    {
        GameSave save = new GameSave(seed, testPlayer);
        save.Random.Next();
        save.Player.Damage(5);

        save.Reset();

        Assert.AreEqual(new Random(seed).Next(), save.Random.Next());
        Assert.AreEqual(startingPlayerHealth, save.Player.Health);
    }
}
