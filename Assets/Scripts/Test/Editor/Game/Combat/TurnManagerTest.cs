using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;

public class TurnManagerTest
{
    Queue<CharacterAction> actionsQueue;
    Mock<CharacterAction> mockCharacterAction;
    GameObject gameObject;
    TurnManager turnManager;

    [SetUp]
    public void Setup()
    {
        actionsQueue = new Queue<CharacterAction>();
        mockCharacterAction = new Mock<CharacterAction>(null, null, null);

        gameObject = new GameObject();
        
        turnManager = gameObject.AddComponent<TurnManager>();
        turnManager.Init(actionsQueue);
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

        Assert.AreEqual(1, actionsQueue.Count);
        Assert.AreEqual(mockCharacterAction.Object, actionsQueue.Peek());
    }

    [Test]
    public void TestGetNextAction()
    {
        actionsQueue.Enqueue(mockCharacterAction.Object);

        CharacterAction action = turnManager.GetNextAction();

        Assert.AreEqual(0, actionsQueue.Count);
        Assert.AreSame(mockCharacterAction.Object, action);
    }

    [Test]
    public void TestShouldWaitForPlayerAction_HasNextAction()
    {
        actionsQueue.Enqueue(mockCharacterAction.Object);

        bool shouldWaitForAction = turnManager.ShouldWaitForPlayerAction();

        Assert.False(shouldWaitForAction);
    }

    [Test]
    public void TestShouldWaitForPlayerAction_NoNextAction()
    {
        bool shouldWaitForAction = turnManager.ShouldWaitForPlayerAction();

        Assert.True(shouldWaitForAction);
    }
}
