using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using NUnit.Framework;
using Moq;

public class CombatManagerTest
{
    Mock<CharacterManager> mockCharacterManager;
    Mock<TurnManager> mockTurnManager;
    Mock<GameFlowManager> mockGameFlowManager;
    Mock<Player> mockPlayer;
    Mock<Enemy> mockEnemy;
    Mock<CharacterAction> mockEnemyAction;
    Mock<Attack> mockPlayerAttack;
    Mock<Defend> mockPlayerDefend;
    GameObject gameObject;
    CombatManager combatManager;
    Queue<CharacterAction> testActionQueue;
    int testNumberEnemyTurns = 1;
    int numberEnemyTurnsTaken = 0;
    int millisecondsToWaitBeforeEnemyAction = 1000;

    [SetUp]
    public void Setup()
    {
        testActionQueue = new Queue<CharacterAction>();
        mockCharacterManager = new Mock<CharacterManager>();
        mockTurnManager = new Mock<TurnManager>();
        mockGameFlowManager = new Mock<GameFlowManager>();
        mockPlayer = new Mock<Player>("name", 100);
        mockEnemy = new Mock<Enemy>("name", 100);
        mockEnemyAction = new Mock<CharacterAction>(mockEnemy.Object, null, null);
        mockPlayerAttack = new Mock<Attack>(null, 5, null, null);
        mockPlayerDefend = new Mock<Defend>(null, 5, null, null);
        mockCharacterManager.Setup(characterManager => characterManager.GetActivePlayer()).Returns(mockPlayer.Object);
        mockCharacterManager.Setup(characterManager => characterManager.GetEnemies()).Returns(new Enemy[] { mockEnemy.Object });
        mockCharacterManager.Setup(characterManager => characterManager.RequestNextAction(It.IsAny<Enemy>())).Returns(mockEnemyAction.Object);
        mockTurnManager.Setup(turnManager => turnManager.RegisterAction(It.IsAny<CharacterAction>())).Callback<CharacterAction>((action) => { testActionQueue.Enqueue(action); });
        mockTurnManager.Setup(turnManager => turnManager.ShouldWaitForPlayerAction()).Returns(() => numberEnemyTurnsTaken >= testNumberEnemyTurns);
        mockTurnManager.Setup(turnManager => turnManager.GetNextAction()).Returns(() => testActionQueue.Count > 0 ? testActionQueue.Dequeue() : null);
        mockEnemyAction.Setup(action => action.Use()).Callback(() => { numberEnemyTurnsTaken++;});
        mockCharacterManager.Setup(characterManager => characterManager.ProcessAction(It.IsAny<CharacterAction>()));
        mockPlayer.Setup(player => player.Attack(It.IsAny<Enemy[]>())).Returns(mockPlayerAttack.Object);
        mockPlayer.Setup(player => player.Defend(It.IsAny<Enemy[]>())).Returns(mockPlayerDefend.Object);
        mockPlayerAttack.Setup(attack => attack.Use());
        mockPlayerDefend.Setup(defend => defend.Use());

        gameObject = new GameObject();

        combatManager = gameObject.AddComponent<CombatManager>();
        combatManager.Init(mockCharacterManager.Object,
                            mockTurnManager.Object,
                            mockGameFlowManager.Object);
    }

    [TearDown]
    public void TearDown()
    {
        numberEnemyTurnsTaken = 0;
    }

    [Test]
    public void TestPlayerAttack()
    {
        combatManager.PlayerAttack();

        Thread.Sleep(millisecondsToWaitBeforeEnemyAction);
        // Simulate coroutine invocation:
        combatManager.ProcessNextAction();

        mockCharacterManager.Verify(manager => manager.GetActivePlayer());
        mockTurnManager.Verify(turns => turns.RegisterAction(mockPlayerAttack.Object));
        mockCharacterManager.Verify(manager => manager.GetEnemies());
        mockPlayer.Verify(player => player.Attack(mockEnemy.Object));
        mockCharacterManager.Verify(manager => manager.RequestNextAction(mockEnemy.Object));
        mockTurnManager.Verify(turns => turns.RegisterAction(mockEnemyAction.Object));
        mockTurnManager.Verify(turns => turns.ShouldWaitForPlayerAction());
        mockTurnManager.Verify(turns => turns.GetNextAction());
        mockCharacterManager.Verify(manager => manager.ProcessAction(mockPlayerAttack.Object));
        mockPlayerAttack.Verify(attack => attack.Use());
        mockCharacterManager.Verify(manager => manager.ProcessAction(mockEnemyAction.Object));
        mockEnemyAction.Verify(action => action.Use());
    }

    [Test]
    public void TestPlayerDefend()
    {
        combatManager.PlayerDefend();

        Thread.Sleep(millisecondsToWaitBeforeEnemyAction);
        // Simulate coroutine invocation:
        combatManager.ProcessNextAction();

        mockCharacterManager.Verify(manager => manager.GetActivePlayer());
        mockTurnManager.Verify(turns => turns.RegisterAction(mockPlayerDefend.Object));
        mockCharacterManager.Verify(manager => manager.GetEnemies());
        mockPlayer.Verify(player => player.Defend(mockEnemy.Object));
        mockCharacterManager.Verify(manager => manager.RequestNextAction(mockEnemy.Object));
        mockTurnManager.Verify(turns => turns.RegisterAction(mockEnemyAction.Object));
        mockTurnManager.Verify(turns => turns.ShouldWaitForPlayerAction());
        mockTurnManager.Verify(turns => turns.GetNextAction());
        mockCharacterManager.Verify(manager => manager.ProcessAction(mockPlayerDefend.Object));
        mockPlayerDefend.Verify(defend => defend.Use());
        mockCharacterManager.Verify(manager => manager.ProcessAction(mockEnemyAction.Object));
        mockEnemyAction.Verify(action => action.Use());
    }

    [Test]
    public void TestProcessNextAction()
    {
        testActionQueue.Enqueue(mockEnemyAction.Object);

        combatManager.ProcessNextAction();

        mockTurnManager.Verify(turns => turns.GetNextAction());
        mockCharacterManager.Verify(manager => manager.ProcessAction(mockEnemyAction.Object));
        mockEnemyAction.Verify(action => action.Use());
    }
}
