using UnityEngine;
using NUnit.Framework;
using Moq;

public class EnemyActionManagerTest
{
    GameObject gameObject;
    EnemyActionManager enemyActionManager;
    Mock<RandomGenerator> mockRandomGenerator;
    Mock<Enemy> mockEnemy;
    Mock<Player> mockPlayer;
    Mock<Attack> mockAttack;
    Mock<Defend> mockDefend;
    Mock<CharacterAction> mockPatternAction;

    [SetUp]
    public void Setup()
    {
        gameObject = new GameObject();

        enemyActionManager = gameObject.AddComponent<EnemyActionManager>();

        mockRandomGenerator = new Mock<RandomGenerator>();
        mockPlayer = new Mock<Player>("name", 100, 1);
        mockEnemy = new Mock<Enemy>("name", 100, 1);
        mockAttack = new Mock<Attack>(null, 5, 5, null, null);
        mockDefend = new Mock<Defend>(null, 5, 5, null, null);
        mockPatternAction = new Mock<CharacterAction>(null, null, null);

        mockEnemy.Setup(enemy => enemy.Attack(mockPlayer.Object)).Returns(mockAttack.Object);
        mockEnemy.Setup(enemy => enemy.Defend(mockPlayer.Object)).Returns(mockDefend.Object);
        mockEnemy.Setup(enemy => enemy.NextActionFromPattern(mockPlayer.Object))
            .Returns(mockPatternAction.Object);

        enemyActionManager.Init(mockRandomGenerator.Object);
    }

    [Test]
    public void TestRequestNextAction_Attack()
    {
        mockRandomGenerator.Setup(generator => generator.Generate(0, 1f)).Returns(1f);

        CharacterAction attack = enemyActionManager.RequestNextAction(mockEnemy.Object, mockPlayer.Object);

        Assert.AreSame(mockAttack.Object, attack);
        mockEnemy.Verify(enemy => enemy.Attack(mockPlayer.Object));
    }

    [Test]
    public void TestRequestNextAction_Defend()
    {
        mockRandomGenerator.Setup(generator => generator.Generate(0, 1f)).Returns(0);

        CharacterAction defend = enemyActionManager.RequestNextAction(mockEnemy.Object, mockPlayer.Object);

        Assert.AreSame(mockDefend.Object, defend);
        mockEnemy.Verify(enemy => enemy.Defend(mockPlayer.Object));
    }

    [Test]
    public void TestRequestNextAction_Pattern()
    {
        mockEnemy.SetupGet(enemy => enemy.HasPattern).Returns(true);

        CharacterAction pattern = enemyActionManager.RequestNextAction(mockEnemy.Object, mockPlayer.Object);

        Assert.AreSame(mockPatternAction.Object, pattern);
        mockEnemy.Verify(enemy => enemy.NextActionFromPattern(mockPlayer.Object));
    }
}
