using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using Moq;

public class VisualAttackQueueTest
{
	VisualAttackQueue attackQueue;
	GameObject gameObject;
	Image[] attackBlocks;
	Mock<TurnManager> mockTurnManager;
	Color playerColor = Color.green;
	Color enemyColor = Color.red;
    Mock<Player> mockPlayer;
    Mock<Enemy> mockEnemy;
    Mock<CharacterAction> mockPlayerAction;
    Mock<CharacterAction> mockEnemyAction;

    [SetUp]
	public void Setup()
	{
		gameObject = new GameObject();
		attackQueue = gameObject.AddComponent<VisualAttackQueue>();
		attackBlocks = new Image[]{
            new GameObject().AddComponent<Image>(),
            new GameObject().AddComponent<Image>(),
            new GameObject().AddComponent<Image>()
        	};
		mockTurnManager = new Mock<TurnManager>();
		attackQueue.Init(attackBlocks,
					  mockTurnManager.Object,
					  playerColor,
					  enemyColor);
        	mockTurnManager.Setup(
			turns => turns.HandleActionsUpdated(It.IsAny<Action>())
		);
        mockPlayer = new Mock<Player>("name", 100, 1);
        mockEnemy = new Mock<Enemy>("name", 100, 1);
        mockPlayerAction = new Mock<CharacterAction>(mockPlayer.Object, null, null);
        mockEnemyAction = new Mock<CharacterAction>(mockEnemy.Object, null, null);
        mockPlayerAction.Setup(action => action.Actor).Returns(mockPlayer.Object);
        mockEnemyAction.Setup(action => action.Actor).Returns(mockEnemy.Object);
        Queue<CharacterAction> queue = new Queue<CharacterAction>();
        queue.Enqueue(mockPlayerAction.Object);
        queue.Enqueue(mockEnemyAction.Object);
        mockTurnManager.Setup(turns => turns.GetQueue()).Returns(queue);
	}

	[Test]
	public void TestAwake()
	{
		attackQueue.Awake();

        for(int i = 0; i < attackBlocks.Length; i++)
        {
            Assert.False(attackBlocks[i].enabled);
        }
        mockTurnManager.Verify(
			turns => turns.HandleActionsUpdated(It.IsAny<Action>())
		);
	}

	[UnityTest]
	public IEnumerator TestUpdateDisplay()
	{
		attackQueue.UpdateDisplay();
		yield return null;

        Assert.True(attackBlocks[0].enabled);
        Assert.AreEqual(playerColor, attackBlocks[0].color);
        Assert.True(attackBlocks[1].enabled);
        Assert.AreEqual(enemyColor, attackBlocks[1].color);
		Assert.False(attackBlocks[2].enabled);
	}
}
