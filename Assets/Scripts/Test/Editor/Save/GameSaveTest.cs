using System;
using NUnit.Framework;

public class GameSaveTest
{
    int seed;
    int startingPlayerHealth = 81;
    Player testPlayer;
    Tutorial combatTutorial;

    [SetUp]
    public void Setup()
    {
        seed = 85;
        testPlayer = new Player("test player", startingPlayerHealth, 2);
        combatTutorial = new Tutorial();
    }

    [Test]
    public void TestInit()
    {
        GameSave save = new GameSave(seed, testPlayer, combatTutorial);

        Assert.AreEqual(new Random(seed).Next(), save.Random.Next());
        Assert.AreSame(testPlayer, save.Player);
    }

    [Test]
    public void TestReset()
    {
        GameSave save = new GameSave(seed, testPlayer, combatTutorial);
        save.Random.Next();
        save.Player.Damage(5);

        save.Reset();

        Assert.AreEqual(new Random(seed).Next(), save.Random.Next());
        Assert.AreEqual(startingPlayerHealth, save.Player.Health);
    }
}
