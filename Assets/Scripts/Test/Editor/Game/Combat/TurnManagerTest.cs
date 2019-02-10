﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;

public class TurnManagerTest
{
    List<Queue<CharacterAction>> actionsQueue;
    List<Dictionary<Character, int>> turnCounts;
    Mock<CharacterAction> mockCharacterAction;
    Mock<Character> mockCharacter;
    Mock<Player> mockPlayer;
    GameObject gameObject;
    TurnManager turnManager;

    [SetUp]
    public void Setup()
    {
        actionsQueue = new List<Queue<CharacterAction>>();
        turnCounts = new List<Dictionary<Character, int>>();
        mockCharacterAction = new Mock<CharacterAction>(null, null, null);
        mockCharacter = new Mock<Character>("name", 100, 1);
        mockPlayer = givenPlayerWithSpeed(1);

        gameObject = new GameObject();
        
        turnManager = gameObject.AddComponent<TurnManager>();
        turnManager.Init(actionsQueue, turnCounts);

        mockCharacterAction.Setup(action => action.Actor).Returns(mockCharacter.Object);
        mockCharacter.Setup(character => character.Speed).Returns(1);
    }

    [TearDown]
    public void TearDown()
    {
        actionsQueue.Clear();
    }

    [Test]
    public void TestRegisterAction()
    {
        turnManager.RegisterAction(mockCharacterAction.Object);

        Assert.AreEqual(1, actionsQueue.First().Count);
        Assert.AreEqual(mockCharacterAction.Object, actionsQueue.First().Peek());
    }

    [Test]
    public void TestGetNextAction()
    {
        actionsQueue.First().Enqueue(mockCharacterAction.Object);

        CharacterAction action = turnManager.GetNextAction();

        Assert.AreEqual(0, actionsQueue.Count);
        Assert.AreSame(mockCharacterAction.Object, action);
    }

    [Test]
    public void TestRegisterAction_GetNextAction_DifferentCharacterSpeeds()
    {
        Mock<Character> slowCharacter = new Mock<Character>("name", 100, 1);
        Mock<Character> fastCharacter = new Mock<Character>("name", 100, 1);
        Mock<CharacterAction> slowCharacterAction = new Mock<CharacterAction>(null, null, null);
        Mock<CharacterAction> fastCharacterAction = new Mock<CharacterAction>(null, null, null);
        slowCharacter.Setup(character => character.Speed).Returns(1);
        fastCharacter.Setup(character => character.Speed).Returns(2);
        slowCharacterAction.Setup(action => action.Actor).Returns(slowCharacter.Object);
        fastCharacterAction.Setup(action => action.Actor).Returns(fastCharacter.Object);

        // Register slow character actions first
        turnManager.RegisterAction(slowCharacterAction.Object);
        turnManager.RegisterAction(slowCharacterAction.Object);
        turnManager.RegisterAction(fastCharacterAction.Object);
        turnManager.RegisterAction(fastCharacterAction.Object);

        // Except the second slow character action to be returned after both fast character actions (based on speed)
        Assert.AreEqual(2, actionsQueue.Count);
        Assert.AreEqual(3, actionsQueue.First().Count);
        Assert.AreSame(slowCharacterAction.Object, turnManager.GetNextAction());
        Assert.AreSame(fastCharacterAction.Object, turnManager.GetNextAction());
        Assert.AreSame(fastCharacterAction.Object, turnManager.GetNextAction());
        Assert.AreSame(slowCharacterAction.Object, turnManager.GetNextAction());
        Assert.AreEqual(0, actionsQueue.Count);
    }

    [Test]
    public void TestShouldWaitForPlayerAction_HasNextAction()
    {
        actionsQueue.First().Enqueue(mockCharacterAction.Object);

        bool shouldWaitForAction = turnManager.ShouldWaitForPlayerAction(mockPlayer.Object);

        Assert.False(shouldWaitForAction);
    }

    [Test]
    public void TestShouldWaitForPlayerAction_NoNextAction()
    {
        bool shouldWaitForAction = turnManager.ShouldWaitForPlayerAction(mockPlayer.Object);

        Assert.True(shouldWaitForAction);
    }

    [Test]
    public void TestShouldWaitForPlayerAction_PlayerSpeed0()
    {
        mockPlayer = givenPlayerWithSpeed(0);

        bool shouldWaitForAction = turnManager.ShouldWaitForPlayerAction(mockPlayer.Object);

        Assert.False(shouldWaitForAction);
    }

    Mock<Player> givenPlayerWithSpeed(int speed)
    {
        Mock<Player> mock = new Mock<Player>("name", 100, speed);
        mock.Setup(player => player.Speed).Returns(speed);
        return mock;
    }
}
